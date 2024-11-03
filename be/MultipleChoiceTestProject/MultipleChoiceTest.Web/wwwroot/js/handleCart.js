window.onload = LoadCart;

function LoadCart() {
    $.ajax({
        url: '/SanPham_62130623/LoadCart',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                var carItems = document.getElementsByClassName("list-group")[0];
                let total = 0;
                for (var v of response.data) {
                    var cartBoxShop = document.createElement("div");
                    cartBoxShop.classList.add("list-group-item", "d-flex", "justify-content-between", "lh-condensed");
                    var cartBoxItem = `
                        <div>
                            <img src="/images/cakes/${v.HinhAnh}" style="width: 80px;"/>
                        </div>
                        <div>
                            <h6 class="my-0" style="max-width: 196px; width: 196px;">${v.TenSP}</h6>
                            <small class="text-muted">${v.DonGia.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) } x ${v.SoLuongMua}</small>
                        </div>
                        <span class="text-muted" style="display: none;">${v.DonGia * v.SoLuongMua}</span>
                    `;
                    total += v.SoLuongMua * v.DonGia;
                    cartBoxShop.innerHTML = cartBoxItem;
                    carItems.append(cartBoxShop);
                }
                var cartTotalHTML = document.createElement("div");
                cartTotalHTML.classList.add("list-group-item", "d-flex", "justify-content-between");
                var totalCart = `
                    <span>Tổng thành tiền</span>
                    <strong>${total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) }</strong>
                `;
                document.getElementById("TotalCart").value = total;
                cartTotalHTML.innerHTML = totalCart;
                carItems.append(cartTotalHTML);
                
            }
        }
    });
}

//$('.btn-checkout').on('click', function () {
//    $.notify("Đặt hàng thành công", "success");
//    setTimeout(function () {
//        $('#form-checkout').submit();
//    }, 1500);
//});

function convertMoney(money) {
    money = money.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
}