document.addEventListener('DOMContentLoaded', function () {
    let xhr = new XMLHttpRequest();
    xhr.open('GET', '/admin/GetAllUser', true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            let users = JSON.parse(xhr.responseText);
            let userListHtml =
                "<table class='table'>" +
                "<thead class='table-display'>" +
                '<tr>' +
                '<th>Id</th>' +
                '<th>Username</th>' +
                '<th>Lastname</th>' +
                '<th>Firstname</th>' +
                '<th>Email</th>' +
                '<th>Action</th>' +
                '</tr>' +
                '</thead>' +
                '<tbody>';

            users.forEach(function (user, index) {
                userListHtml +=
                    '<tr>' +
                    "<td  scope='row'> <span class='res-head'>" +
                    (index + 1) +
                    '</span></td>' +
                    "<td> <span class='res-head'>" +
                    user.userName +
                    '</span></td>' +
                    "<td > <span class='res-head'>" +
                    user.firstName +
                    ' </span></td>' +
                    "<td> <span class='res-head'>" +
                    user.lastName +
                    '</span></td>' +
                    "<td> <span class='res-head'>" +
                    user.email +
                    '</span></td>' +
                    "<td class='action'>" +
                    '<button>Edit</button>' +
                    '<button>Delete</button>' +
                    '</td>' +
                    '</tr>';
            });
            userListHtml += '</tbody>' + '</table>';
            let userList = document.getElementById('userList');
            userList.innerHTML = userListHtml;
        } else if (xhr.readyState === 4) {
            alert('Failed to get user list.');
        }
    };
    xhr.send();
});



/*$(document).ready(function () {
  $.ajax({
    url: '/admin/GetAllUser',
    type: 'GET',
    dataType: 'json',
    success: function (users) {
      var userListHtml =
        "<table class='table'>" +
        "<thead class='table-display'>" +
        '<tr>' +
        '<th>Id</th>' +
        '<th>Username</th>' +
        '<th>Lastname</th>' +
        '<th>Firstname</th>' +
        '<th>Email</th>' +
        '<th>Action</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>';

      users.forEach(function (user, index) {
        userListHtml +=
          '<tr>' +
          "<td  scope='row'> <span class='res-head'>" +
          (index + 1) +
          '</span></td>' +
          "<td> <span class='res-head'>" +
          user.userName +
          '</span></td>' +
          "<td > <span class='res-head'>" +
          user.firstName +
          ' </span></td>' +
          "<td> <span class='res-head'>" +
          user.lastName +
          '</span></td>' +
          "<td> <span class='res-head'>" +
          user.email +
          '</span></td>' +
          "<td class='action'>" +
          '<button>Edit</button>' +
          '<button>Delete</button>' +
          '</td>' +
          '</tr>';
      });
      userListHtml += '</tbody>' + '</table>';
      $('#userList').html(userListHtml);
    },
    error: function () {
      alert('Failed to get user list.');
    },
  });
});*/
