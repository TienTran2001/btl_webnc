function loadListProduct() {
  let xhr = new XMLHttpRequest();
  xhr.open('GET', '/Admin/GetAllProducts', true);
  xhr.setRequestHeader('Content-Type', 'application/json');
  xhr.onreadystatechange = function () {
    if (xhr.readyState === 4 && xhr.status === 200) {
      let result = JSON.parse(xhr.responseText);
      const { products } = result;

      let productListHtml = `
      <table class='table' id='#userTable'>
        <thead class='table-display'>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Image</th>
           
            <th>Author</th>
            <th>Price</th>
            <th>quantity</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
      `;
      products.forEach(function (product, index) {
        const format = product.price.toLocaleString('vi-VN') + 'đ';
        productListHtml += `
        <tr>
          <td  scope='row'> <span class='res-head'>${index + 1}</span></td>
          <td> <span class='res-head'>${product.name}</span></td>
          <td class="td-img"> <span class='res-head'><img class='product-image' src="/uploads/${
            product.imagePath
          }" /></span></td>        
          <td> <span class='res-head'>${product.author}</span></td>
          <td> <span class='res-head'>${format}</span></td>
          <td> <span class='res-head'>${product.quantity}</span></td>
          <td class='action'>
            <span class='action-wrap'>
              <button class="btn-update" data-product-id=${
                product.id
              } ><i class="bi bi-pencil"></i> Edit</button>
              <button class="btn-delete" data-product-id=${
                product.id
              }><i class="bi bi-trash"></i> Delete</button>  
            </span>
          </td>
        </tr>
      `;
      });
      productListHtml += '</tbody>' + '</table>';
      let productList = document.getElementById('productList');
      if (productList) productList.innerHTML = productListHtml;
      // handleDeleteUser();
      handleUpdateProduct();
    } else if (xhr.readyState === 4) {
    }
  };
  xhr.send();
}

function handleUpdateProduct() {
  const btnEdits = document.querySelectorAll('.btn-update');
  const popupEditProduct = document.querySelector('.popup-edit');
  const overlay = document.querySelector('.overlay');
  const cancelBtn = document.getElementById('cancelUpdateProduct');

  const name = document.querySelector('.update-name');
  const image = document.querySelector('.update-image');
  const description = document.querySelector('.update-description');
  const price = document.querySelector('.update-price');
  const author = document.querySelector('.update-author');
  const category = document.querySelector('.update-category');
  const quantity = document.querySelector('.update-quantity');

  const confirmUpdate = document.querySelector('#confirmUpdate');

  btnEdits.forEach((item) => {
    item.onclick = () => {
      popupEditProduct.classList.add('show');
      overlay.classList.add('show');

      const productId = item.getAttribute('data-product-id');
      let xhr = new XMLHttpRequest();
      xhr.open('GET', `/admin/GetProductById?id=${productId}`, true);
      xhr.setRequestHeader('Content-Type', 'application/json');
      xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
          let { success, product } = JSON.parse(xhr.responseText);
          name.value = product.name;
          description.value = product.description;
          price.value = product.price;
          author.value = product.author;
          category.value = product.category;
          quantity.value = product.quantity;

          if (success) {
            confirmUpdate.onclick = () => {
              console.log('click');
              var formData = new FormData();
              formData.append('imageFile', image.files[0]);
              formData.append('name', name.value);
              formData.append('id', product.id);
              formData.append('description', description.value);
              formData.append('price', price.value);
              formData.append('author', author.value);
              formData.append('category', category.value);
              formData.append('quantity', quantity.value);
              $.ajax({
                url: '/Admin/UpdateProduct',
                type: 'PUT',
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                  // Xử lý thành công
                  // console.log(product);
                  if (success) {
                    alert('Sửa thành công!!!');
                    cancelBtn.click();
                    console.log(product);
                    loadListProduct();
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
    popupEditProduct.classList.remove('show');
    overlay.classList.remove('show');
  };
}

function app() {
  $(document).ready(function () {
    loadListProduct();
  });
}

app();
