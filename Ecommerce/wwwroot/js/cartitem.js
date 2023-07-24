function loadListCart() {
  let xhr = new XMLHttpRequest();
  xhr.open('GET', '/CartItem/GetAllCart', true);
  xhr.setRequestHeader('Content-Type', 'application/json');
  xhr.onreadystatechange = function () {
    if (xhr.readyState === 4 && xhr.status === 200) {
      let result = JSON.parse(xhr.responseText);
      const { count, carts } = result;
      // console.log(carts);
      let productListHtml = `
      <table class="cart-table">
        <thead>
            <tr>
                <th>Image</th>
                <th>Name</th>
                <th>Price</th>
                <th>Quantity</th>
                <th>Total Price</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
      `;
      let total = 0;
      carts.forEach(function (cart) {
        // const format = product.price.toLocaleString('vi-VN') + 'đ';
        const totalItem = cart.quantity * cart.price;
        total += totalItem;
        productListHtml += `
        <tr>
                <td class="cart-img"><img src="/uploads/${cart.imagePath}" alt=""></td>
                <td class="cart-name">${cart.productName}</td>
                <td class="cart-price">${cart.price}</td>
                <td class="cart-quantity"><input type="number" value=${cart.quantity} min="1"></td>
                <td class="cart-item-total">${totalItem}</td>
                <td class="cart-action">
                  <button class="cart-btn-update" data-cart-id=${cart.id}>cập nhật</button>
                  <button class="cart-btn-delete" data-cart-id=${cart.id}>Xóa</button>
                </td>
            </tr>
      `;
      });
      console.log(total);
      productListHtml += '</tbody>' + '</table>';
      let productList = document.getElementById('cartList');
      let cartCount = document.getElementById('cartCount');
      cartCount.innerHTML = count;
      if (productList) productList.innerHTML = productListHtml;
      updateToCart();
      deleteCart();
      addToCart();
    } else if (xhr.readyState === 4) {
    }
  };
  xhr.send();
}

function addToCart() {
  const btnAddToCart = document.querySelector('.add-detail-cart');
  const quantity = document.querySelector('.book-detail-quantity');
  const pid = btnAddToCart.getAttribute('data-product-id');

  btnAddToCart.onclick = function () {
    $.ajax({
      url: '/CartItem/AddToCart',
      type: 'POST',
      data: { productId: pid, quantity: quantity.value },
      success: function (result) {
        if (result.success == true) {
          alert(result.message);
          loadListCart();
        } else {
          alert('Ko duoc');
        }
      },
      error: function () {
        // Xử lý lỗi
      },
    });
  };
}

function updateToCart() {
  const quantity = document.querySelectorAll('.cart-quantity input');
  const btnUpdates = document.querySelectorAll('.cart-btn-update');

  btnUpdates.forEach(function (button) {
    button.addEventListener('click', function () {
      const row = button.closest('tr');
      const quantity = row.querySelector('.cart-quantity input').value;
      const cid = button.getAttribute('data-cart-id');
      console.log(cid);
      $.ajax({
        url: '/CartItem/UpdateToCart',
        type: 'POST',
        data: { cartId: cid, quantity: quantity },
        success: function (result) {
          if (result.success == true) {
            alert('Cập nhật thành công');
            loadListCart();
          } else {
            alert('Ko duoc');
          }
        },
        error: function () {
          // Xử lý lỗi
        },
      });
    });
  });
}

function deleteCart() {
  const btnDeletes = document.querySelectorAll('.cart-btn-delete');
  btnDeletes.forEach((button) => {
    button.onclick = function () {
      const cid = button.getAttribute('data-cart-id');
      $.ajax({
        url: `/CartItem/DeleteCart?cartId=${cid}`,
        type: 'DELETE',
        success: function (data) {
          if (data.success) {
            loadListCart();
          } else {
            alert(data.message);
          }
        },
        error: function () {
          // Xử lý khi xảy ra lỗi
        },
      });
    };
  });
}

function app() {
  $(document).ready(function () {
    loadListCart();
  });
}
app();
