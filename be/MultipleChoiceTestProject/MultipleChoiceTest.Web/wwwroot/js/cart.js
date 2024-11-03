window.onload = LoadCart;


function LoadCart() {
    $.ajax({
        url: '/Carts/LoadCart',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                for (var v of response.data.data) {
                    //total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                    var cartShopBox = document.createElement("div");
                    cartShopBox.classList.add("cart-box");
                    var cartItems = document.getElementsByClassName("cart-content")[0];

                    var cartBoxContent = `
                        <input type="hidden" value="${v.id}" class="detail-id" name="Id"/>
                        <input type="hidden" value="${v.productId}" class="product-id" name="ProductId"/>
                        <img src="/images/product/${v.image}" name="Image" alt="" class="cart-img">
                        <div class="detail-box">
                            <div class="cart-product-title" name="ProductName">${v.productName}</div>
                            <div class="cart-price" name="DonGia">${v.price}</div>
                            <input type="number" value="${v.quantity}" class="cart-quantity" name="Quantity" min="0" max="10">
                        </div>
                        <!-- Remove Cart -->
                        <i class='bx bxs-trash-alt cart-remove' ></i>`;

                    cartShopBox.innerHTML = cartBoxContent;
                    cartItems.append(cartShopBox);
                    cartShopBox
                        .getElementsByClassName("cart-remove")[0]
                        .addEventListener("click", removeCartItem);
                    cartShopBox
                        .getElementsByClassName("cart-quantity")[0]
                        .addEventListener("change", quantityChanged);
                }
                updatetotal();
            }
        }, error: function (err) {
            console.log(err);
        }
    });
}

//  Cart
let cartIcon = document.querySelector("#cart-icon");
let cart = document.querySelector(".cart");
let closeCart = document.querySelector("#close-cart");
// Open Cart
cartIcon.onclick = () => {
    cart.classList.add("active");
};
// Close Cart
closeCart.onclick = () => {
    cart.classList.remove("active");
};

// Cart Working JS
if (document.readyState == "loading") {
    document.addEventListener("DOMContentLoaded", ready);
} else {
    ready();
}

// Making Function
function ready() {
    // Reomve Items From Cart
    var reomveCartButtons = document.getElementsByClassName("cart-remove");
    console.log(reomveCartButtons);
    for (var i = 0; i < reomveCartButtons.length; i++) {
        var button = reomveCartButtons[i];
        button.addEventListener("click", removeCartItem);
    }
    // Quantity Changes
    var quantityInputs = document.getElementsByClassName("cart-quantity");
    for (var i = 0; i < quantityInputs.length; i++) {
        var input = quantityInputs[i];
        input.addEventListener("change", quantityChanged);
    }
    // Add To Cart
    var addCart = document.getElementsByClassName("add-cart");
    for (var i = 0; i < addCart.length; i++) {
        var button = addCart[i];
        button.addEventListener("click", addCartClicked);
    }

    // Buy Button Work
    document
        .getElementsByClassName("btn-buy")[0]
        .addEventListener("click", buyButtonClicked);
}
// Buy Button
function buyButtonClicked() {
    var cartContent = document.getElementsByClassName("cart-content")[0];
    if (cartContent.innerHTML == "") {
        alert('Please purchase before payment');
    } else {
        window.location.href = '/Pays/Index';
    }
}

// Reomve Items From Cart
function removeCartItem(event) {
    if (confirm('Are you sure you want to delete this product?')) {
        var buttonClicked = event.target;
        var idDetail = buttonClicked.parentElement.getElementsByClassName("detail-id")[0].value;
        RemoveCartInDB(idDetail);
    }
}

function RemoveCartInDB(idDetail) {
    $.ajax({
        url: '/Carts/RemoveCart',
        type: 'POST',
        dataType: 'json',
        data: {
            detailId: idDetail
        },
        success: function (response) {
            if (!response.success) {
                alert(response.message);
            } else {
                alert('Remove product successfully');
                document.getElementsByClassName("cart-content")[0].innerHTML = "";
                LoadCart();
                updatetotal();
            }
        }
    })
}

// Quantity Changes
function quantityChanged(event) {
    var input = event.target;
    if (isNaN(input.value) || input.value <= 0) {
        input.value = 1;
    }

    var detailId = $(input).closest('.cart-box').find('.detail-id')[0];
    console.log(detailId.value);
    updateCartDetail(detailId.value, input.value);
    updatetotal();
}
// Add To cart

function addCartClicked(event) {
    var button = event.target;
    var shopProducts = button.parentElement;
    var productId = shopProducts.getElementsByClassName("product-id")[0].value;

    AddProductInDb(productId);
    updatetotal();
}

function AddProductInDb(productId) {
    $.ajax({
        url: '/Carts/InsertCart',
        type: 'POST',
        dataType: 'json',
        data: {
            idProduct: productId
        },
        success: function (response) {
            switch (response.data) {
                case 1:
                    alert('Insert product successfully');
                    // remove product
                    const listCart = document.getElementsByClassName("cart-content");
                    listCart[0].innerHTML = "";
                    LoadCart();
                    updatetotal();
                    break;
                case 0:
                    alert('Error! Please contact the consulting center');
                    break;
                case -1:
                    alert('Please log in');
                    window.location.href = "/Auth/Login";
                    break;
            }
        }
    });
}

function updateCartDetail(cartDetailId, quantityProduct) {
    $.ajax({
        url: '/Carts/UpdateQuantityCartDetail',
        type: 'POST',
        dataType: 'json',
        data: {
            detailId: cartDetailId,
            quantity: quantityProduct
        },
        success: function (response) {
            if (!response.success) {
                alert('Please log in');
                window.location.href = "/Auth/Login";
            }
        }
    });
}

// Update Total
function updatetotal() {
    var cartContent = document.getElementsByClassName("cart-content")[0];
    var cartBoxes = cartContent.getElementsByClassName("cart-box");
    var total = 0;
    for (var i = 0; i < cartBoxes.length; i++) {
        var cartBox = cartBoxes[i];
        var priceElement = cartBox.getElementsByClassName("cart-price")[0];
        var quantityElement = cartBox.getElementsByClassName("cart-quantity")[0];
        var price = parseFloat(priceElement.innerText.replace("VND", ""));
        var quantity = quantityElement.value;
        total = total + price * quantity;
    }
    // If price Contain some Cents Value
    total = Math.round(total * 100) / 100;

    document.getElementsByClassName("total-price")[0].innerText = total.toLocaleString('it-IT', { style: 'currency', currency: 'USD' });
}


