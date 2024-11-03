
$(document).ready(function () {
    $("#pickupImage").change(function () {
        var maLoai = document.getElementById("MaLoaiSP").value;
        var file = $("#pickupImage")[0].files[0];
    });

    function uploadImage(file, maLoaiSP) {
        var formData = new FormData();
        formData.append("file", file);
        console.log(formData);
        $.ajax({
            url: '/Admin/SanPhamAdmin/SaveFile',
            type: 'POST',
            data: {
                file: file,
                maLoaiSP: maLoaiSP
            },
            dataType: 'json',
            cache: false,
            processData: false,
            success: function (urlImage) {
                document.getElementById("HinhAnh").value = urlImage;
                ViewImage(urlImage);
            }
        });
    }

    

    function ViewImage(url) {
        if (url) {
            document.getElementById("imageView").attr('src', url);
        }
    }
});