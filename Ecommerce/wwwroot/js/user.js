function loadListUser() {
  let xhr = new XMLHttpRequest();
  xhr.open('GET', '/admin/GetAllUser', true);
  xhr.setRequestHeader('Content-Type', 'application/json');
  xhr.onreadystatechange = function () {
    if (xhr.readyState === 4 && xhr.status === 200) {
      let users = JSON.parse(xhr.responseText);
      let userListHtml = `
      <table class='table' id='#userTable'>
        <thead class='table-display'>
          <tr>
            <th>Id</th>
            <th>Username</th>
            <th>Lastname</th>
            <th>Firstname</th>
            <th>Email</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
      `;
      users.forEach(function (user, index) {
        userListHtml += `
        <tr>
          <td  scope='row'> <span class='res-head'>${index + 1}</span></td>
          <td> <span class='res-head'>${user.userName}</span></td>
          <td> <span class='res-head'>${user.firstName}</span></td>
          <td> <span class='res-head'>${user.lastName}</span></td>
          <td> <span class='res-head'>${user.email}</span></td>
          <td class='action'>
            <a href="/Admin/UpdateUser">Edit</a>
            <button class="btn-delete-user" data-user-id=${
              user.id
            }><i class="bi bi-trash"></i> Delete</button>
          </td>
        </tr>
      `;
      });
      userListHtml += '</tbody>' + '</table>';
      let userList = document.getElementById('userList');
      userList.innerHTML = userListHtml;
      handleDeleteUser();
    } else if (xhr.readyState === 4) {
    }
  };
  xhr.send();
}
function handleDeleteUser() {
  const btnDeleteUsers = document.querySelectorAll('.btn-delete-user');
  const overlay = document.querySelector('.overlay');
  const cancelBtn = document.getElementById('cancelDelete');
  const confirmationPopup = document.getElementById('confirmationPopup');
  const confirmDelete = document.getElementById('confirmDelete');

  btnDeleteUsers.forEach((item) => {
    item.onclick = () => {
      confirmationPopup.classList.add('show');
      overlay.classList.add('show');
      const userId = item.getAttribute('data-user-id');
      confirmDelete.onclick = () => {
        $.ajax({
          url: '/Admin/DeleteUser',
          type: 'POST',
          data: { id: userId },
          success: function (result) {
            // Xử lý thành công
            console.log(result);
            if (result.statusCode == 1) {
              alert(result.message);
              cancelBtn.click();
              console.log(cancelBtn);
              loadListUser();
            } else {
              alert(result.message);
            }
          },
          error: function () {
            // Xử lý lỗi
          },
        });
        // handle delete
      };
    };
  });

  cancelBtn.onclick = () => {
    confirmationPopup.classList.remove('show');
    overlay.classList.remove('show');
  };
}

function load() {
  $.ajax({
    url: '/admin/GetAllUser',
    type: 'GET',
    dataType: 'json',
    success: function (users) {
      let userListHtml = `
      <table class='table'>
        <thead class='table-display'>
          <tr>
            <th>Id</th>
            <th>Username</th>
            <th>Lastname</th>
            <th>Firstname</th>
            <th>Email</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
      `;
      users.forEach(function (user, index) {
        userListHtml += `
        <tr>
          <td  scope='row'> <span class='res-head'>${index + 1}</span></td>
          <td> <span class='res-head'>${user.userName}</span></td>
          <td> <span class='res-head'>${user.firstName}</span></td>
          <td> <span class='res-head'>${user.lastName}</span></td>
          <td> <span class='res-head'>${user.email}</span></td>
          <td class='action'>
            <a href="/Admin/UpdateUser">Edit</a>
            <button class="btn-delete-user" data-user-id=${
              user.id
            }><i class="bi bi-trash"></i> Delete</button>
          </td>
        </tr>
      `;
      });
      userListHtml += '</tbody>' + '</table>';
      let userList = document.getElementById('userList');
      userList.innerHTML = userListHtml;
    },
    error: function () {
      alert('Failed to get user list.');
    },
  });
}

function app() {
  $(document).ready(function () {
    loadListUser();
  });
}

app();
