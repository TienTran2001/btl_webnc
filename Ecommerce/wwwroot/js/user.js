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
            <button class="btn-update-user" data-user-id=${
              user.id
            } ><i class="bi bi-pencil"></i> Edit</button>
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
      handleUpdateUser();
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
          type: 'DELETE',
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
      };
    };
  });

  cancelBtn.onclick = () => {
    confirmationPopup.classList.remove('show');
    overlay.classList.remove('show');
  };
}

function handleUpdateUser() {
  const btnEdits = document.querySelectorAll('.btn-update-user');
  const popupEditUser = document.querySelector('.popup-edit-user');
  const overlay = document.querySelector('.overlay');
  const cancelBtn = document.getElementById('cancelUpdate');

  const firstName = document.querySelector('.update-first-name');
  const lastName = document.querySelector('.update-last-name');
  const email = document.querySelector('.update-email');
  const username = document.querySelector('.update-username');
  const confirmUpdate = document.querySelector('#confirmUpdate');

  console.log(overlay);

  btnEdits.forEach((item) => {
    item.onclick = () => {
      popupEditUser.classList.add('show');
      overlay.classList.add('show');

      const userId = item.getAttribute('data-user-id');
      console.log(userId);
      let xhr = new XMLHttpRequest();
      xhr.open('PUT', `/admin/FindUserById?id=${userId}`, true);
      xhr.setRequestHeader('Content-Type', 'application/json');
      xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
          let result = JSON.parse(xhr.responseText);
          console.log(result);
          if (result.statusCode == 1) {
            firstName.value = result.user.firstName;
            lastName.value = result.user.lastName;
            email.value = result.user.email;
            username.value = result.user.userName;

            confirmUpdate.onclick = () => {
              $.ajax({
                url: '/Admin/UpdateUser',
                type: 'POST',
                data: {
                  id: result.user.id,
                  firstName: firstName.value,
                  lastName: lastName.value,
                  email: email.value,
                  username: username.value,
                },
                success: function (result) {
                  // Xử lý thành công
                  console.log(result);
                  if (result.statusCode == 1) {
                    alert('Sửa thành công!!!');
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
            };
          }
        } else if (xhr.readyState === 4) {
          console.log('err');
        }
      };
      xhr.send();
    };
  });

  cancelBtn.onclick = () => {
    popupEditUser.classList.remove('show');
    overlay.classList.remove('show');
  };
}

// search
function handleSearchUser() {
  const searchInput = document.querySelector('.search');
  let debounceTimeout;

  searchInput.addEventListener('input', function () {
    // Hủy bỏ timeout trước đó (nếu có)
    clearTimeout(debounceTimeout);

    // Đặt timeout mới để gọi hàm search sau 500ms
    debounceTimeout = setTimeout(function () {
      const searchTerm = searchInput.value.trim();
      if (searchTerm == '') {
        loadListUser();
      } else {
        searchUsersByUsername(searchTerm);
      }
    }, 500);
  });
}

function searchUsersByUsername(searchTerm) {
  let xhr = new XMLHttpRequest();
  xhr.open('GET', `/Admin/SearchUser?username=${searchTerm}`, true);
  xhr.setRequestHeader('Content-Type', 'application/json');
  xhr.onreadystatechange = function () {
    if (xhr.readyState === 4 && xhr.status === 200) {
      let data = JSON.parse(xhr.responseText);
      const { users } = data;
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
            <button class="btn-update-user" data-user-id=${
              user.id
            } ><i class="bi bi-pencil"></i> Edit</button>
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
      handleUpdateUser();
    } else if (xhr.readyState === 4) {
    }
  };
  xhr.send();
}

function app() {
  $(document).ready(function () {
    loadListUser();
    handleSearchUser();
  });
}

app();
