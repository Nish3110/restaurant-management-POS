function getCookie(cname) {
    let name = cname + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function AddtoCookie(id, ITEM_DESC, Qty, SALE_PRICE, ItemID) {
    //var Product = [
    //    { 'Sr': 1, 'age': 1 },
    //    { 'Sr': 'Bella', 'age': 2 },
    //    { 'Sr': 'Chad', 'age': 3 },
    //];
    var i = getCookie("Product");
    if (i == "") {
        var Product = [
            { 'Sr': 1, 'ProductID': id, 'Quantity': Number(Qty), 'Price': SALE_PRICE, 'Item_Description': ITEM_DESC, 'ComboID': ItemID }
        ];
        setCookie("Product", JSON.stringify(Product), 7);
    }
    else {
        var Product = JSON.parse(getCookie("Product"));
        if (Product.length > 0) {

            var sno = Number(Product[Product.length - 1].Sr) + Number(1);

            Product.push(
                { 'Sr': sno, 'ProductID': id, 'Quantity': Number(Qty), 'Price': SALE_PRICE, 'Item_Description': ITEM_DESC, 'ComboID': ItemID }
            );
            setCookie("Product", JSON.stringify(Product), 7);
        }
        else {
            var Product = [
                { 'Sr': 1, 'ProductID': id, 'Quantity': Number(Qty), 'Price': SALE_PRICE, 'Item_Description': ITEM_DESC, 'ComboID': ItemID }
            ];
            setCookie("Product", JSON.stringify(Product), 7);
        }
    }
}
function RemoveFromCookie(id, ComboID) {
    const Product = JSON.parse(getCookie("Product"));

    if (ComboID == "0") {
        $(Product).each(function (i, v) {
            if (v && v.Sr == id) {
                Product.splice(i, 1);
                setCookie("Product", JSON.stringify(Product), 7);
            }
        });
    }
    else {
        //$(Product).each(function (i, v) {
        for (var i = 0; Product.length > i; i++) {
            if (Product[i].ComboID == ComboID) {
                Product.splice(i, 1);
                i--;
            }
        };
        setCookie("Product", JSON.stringify(Product), 7);
    }
    //refill
    if (Product.length > 0) {
        $("#fillcartitem").html('');
        $("#total").html('');
        var total = 0.0;
        var Fill = '';
        Fill =
            '<tr>' +
            '<th>ITEM</th>' +
            '<th>RATE</th>' +
            '<th class="text-center">QTY</th>' +
            '</tr>' +
            '<tr>';

        for (var i = 0; i < Product.length; i++) {
            total += Product[i].Price * Product[i].Quantity;
            Fill += '<td>';

            Fill += '<h4>' + Product[i].Item_Description + '</h4>';

            Fill += '</td>' +
                '<td>' + Product[i].Price + '</td>' +
                '<td style="width:35%;">' +
                '<div class="input-group">' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number" href="#1" onclick="MinusQtyCookie(this.id)">' +
                '  -' +
                '</button>' +
                '</span>' +
                '<div class="start" >' +
                '<input type="number" style="text-align: center;" required="" name="QuranTilawat_Sipara" class="form-control input-number ng-pristine ng-untouched ng-valid ng-valid-min ng-valid-required" value="' + Product[i].Quantity + '" min="1" disabled="disabled">' +
                '</div>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1" onclick="AddQtyCookie(this.id)">' +
                '  +' +
                '</button>' +
                '</span>' +
                '</div>' +
                '</td>' +
                '<td>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1"  onclick="RemoveFromCookie(this.id,\'' + Product[i].ComboID + '\');">' +
                'x' +
                '</button>' +
                '</span>' +
                '</td>' +
                '</tr>'
        }
        $("#fillcartitem").append(Fill);
        $("#total").append(total);

    }
    else {
        location.reload();
    }
}
function AddQtyCookie(id) {

    const Product = JSON.parse(getCookie("Product"));
    $(Product).each(function (i, v) {
        if (v && v.Sr == id && v.ComboID == "0") {
            var qty = Number(v.Quantity) + Number(1);
            v.Quantity = qty;
            setCookie("Product", JSON.stringify(Product), 7);
        }
    });
    //Refill
    if (Product.length > 0) {
        $("#fillcartitem").html('');
        $("#total").html('');
        var total = 0.0;
        var Fill = '';
        Fill =
            '<tr>' +
            '<th>ITEM</th>' +
            '<th>RATE</th>' +
            '<th class="text-center">QTY</th>' +
            '</tr>' +
            '<tr>';

        for (var i = 0; i < Product.length; i++) {
            total += Product[i].Price * Product[i].Quantity;
            Fill += '<td>';

            Fill += '<h4>' + Product[i].Item_Description + '</h4>';

            Fill += '</td>' +
                '<td>' + Product[i].Price + '</td>' +
                '<td style="width:35%;">' +
                '<div class="input-group">' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number" href="#1" onclick="MinusQtyCookie(this.id)">' +
                '  -' +
                '</button>' +
                '</span>' +
                '<div class="start" >' +
                '<input type="number" style="text-align: center;" required="" name="QuranTilawat_Sipara" class="form-control input-number ng-pristine ng-untouched ng-valid ng-valid-min ng-valid-required" value="' + Product[i].Quantity + '" min="1" disabled="disabled">' +
                '</div>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1" onclick="AddQtyCookie(this.id)">' +
                '  +' +
                '</button>' +
                '</span>' +
                '</div>' +
                '</td>' +
                '<td>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1"  onclick="RemoveFromCookie(this.id,\'' + Product[i].ComboID + '\');">' +
                'x' +
                '</button>' +
                '</span>' +
                '</td>' +
                '</tr>'
        }
        $("#fillcartitem").append(Fill);
        $("#total").append(total);

    }
}
function MinusQtyCookie(id) {
    const Product = JSON.parse(getCookie("Product"));
    $(Product).each(function (i, v) {
        if (v && v.Sr == id && v.ComboID == "0") {
            var qty = Number(v.Quantity) - Number(1);
            if (qty == 0) {
                v.Quantity = 1;
            }
            else {
                v.Quantity = qty;
            }
            setCookie("Product", JSON.stringify(Product), 7);
        }
    });
    //Refill
    if (Product.length > 0) {
        $("#fillcartitem").html('');
        $("#total").html('');
        var total = 0.0;
        var Fill = '';
        Fill =
            '<tr>' +
            '<th>ITEM</th>' +
            '<th>RATE</th>' +
            '<th class="text-center">QTY</th>' +
            '</tr>' +
            '<tr>';

        for (var i = 0; i < Product.length; i++) {
            total += Product[i].Price * Product[i].Quantity;
            Fill += '<td>';

            Fill += '<h4>' + Product[i].Item_Description + '</h4>';

            Fill += '</td>' +
                '<td>' + Product[i].Price + '</td>' +
                '<td style="width:35%;">' +
                '<div class="input-group">' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number" href="#1" onclick="MinusQtyCookie(this.id)">' +
                '  -' +
                '</button>' +
                '</span>' +
                '<div class="start" >' +
                '<input type="number" style="text-align: center;" required="" name="QuranTilawat_Sipara" class="form-control input-number ng-pristine ng-untouched ng-valid ng-valid-min ng-valid-required" value="' + Product[i].Quantity + '" min="1" disabled="disabled">' +
                '</div>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1" onclick="AddQtyCookie(this.id)">' +
                '  +' +
                '</button>' +
                '</span>' +
                '</div>' +
                '</td>' +
                '<td>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1"  onclick="RemoveFromCookie(this.id,\'' + Product[i].ComboID + '\');">' +
                'x' +
                '</button>' +
                '</span>' +
                '</td>' +
                '</tr>'
        }
        $("#fillcartitem").append(Fill);
        $("#total").append(total);

    }
}
function AddtoCart(id, ITEM_DESC, Qty, SALE_PRICE, ItemID, Product_Mode) {
    var _id = "#Qty" + Qty;
    Qty = $(_id).val();
    //}
    $.ajax({
        type: "post",
        url: "/RTC/AddCART",
        data: { 'id': id, 'Product_Mode': Product_Mode },
        dataType: "json",
        traditional: true,
        success: function (data) {
            if (data.IsOpenQty != null) {
                if (data.IsOpenQty) {
                    //open div
                    document.getElementById("txtopenqty").value = 1;
                    document.getElementById("txt_itm_id").value = data.itm_id;
                    $('#openqty').css('display', 'block');
                    return;
                }
            }
            if (data.IsOpen != null) {
                if (data.IsOpen) {
                    //open div
                    document.getElementById("OpenItemID").value = id;
                    $('#openitem').css('display', 'block');
                    return;
                }
            }
            if (data.isRedirect) {
                AddtoCookie(id, ITEM_DESC, Qty, SALE_PRICE, ItemID);
                window.location.href = data.redirectUrl + "/" + data.mid;
                return;
            }
            if (data == "Normal") {
                AddtoCookie(id, ITEM_DESC, Qty, SALE_PRICE, "0");
                const Product = JSON.parse(getCookie("Product"));

                $("#TXTAlertforcart").css("display", "block");
                $('#cartcount').html('');
                $("#cartcount").append('<button type="button" class="button-78" id="open_cart" style="float:right;" href="#1" onclick="ShowCartItem()"><i class="icon-food"></i> : ' + Product.length + '</button>');

                setTimeout(function () {
                    $("#TXTAlertforcart").css("display", "none");
                }, 1000);
            }

        },
        error: function (request, status, error) {
            location.reload();
        }

    });
}
function AddtoCartOpenQty() {
    var qty = $('#txtopenqty').val();
    var x = localStorage.getItem("Lang");

    $.ajax({
        type: "post",
        url: "/RTC/AddOpenqty?qty=" + qty + "&id=" + $("#txt_itm_id").val(),
        // data: { id: id, },
        data: {},
        dataType: "json",
        traditional: true,
        success: function (data) {
            $('#cartcount').html('');
            $("#cartcount").append('<button type="button" class="button-78" id="open_cart" style="float:right;" href="#1" onclick="ShowCartItem()"><i class="icon-food"></i> : ' + data.lineCollection.length + '</button>');
        }
        //2022/2/2
        , error: function (request, status, error) {
            location.reload();
        }
    });
}

function AddtoCartOpen() {
    //txtitemname txtitemrate
    var id = $('#OpenItemID').val();;
    var itemname = $('#txtitemname').val();
    var rate = $('#txtitemrate').val();
    if (rate != "" && itemname != "") {
        AddtoCookie(id, itemname, 1, rate, "0");
        $('#openitem').css('display', 'none');
    }
    const Product = JSON.parse(getCookie("Product"));
    $('#cartcount').html('');
    $("#cartcount").append('<button type="button" class="button-78" id="open_cart" style="float:right;" href="#1" onclick="ShowCartItem()"><i class="icon-food"></i> : ' + Product.length + '</button>');

}

function Cartcount() {
    //    url: "/RTC/CART_COUNT",
    i = getCookie("Product");
    $('#cartcount').html('');
    if (i == "") {
        $("#cartcount").append('<button type="button" class="button-78" id="open_cart" style="float:right;" href="#1" onclick="ShowCartItem()"><i class="icon-food"></i> : 0</button>');
    }
    else {
        const Product = JSON.parse(getCookie("Product"));
        $("#cartcount").append('<button type="button" class="button-78" id="open_cart" style="float:right;" href="#1" onclick="ShowCartItem()"><i class="icon-food"></i> : ' + Product.length + '</button>');
    }
}
//ShowCart-------------------------------------------------------------------------------------
function ShowCartItem() {
    const Product = JSON.parse(getCookie("Product"));
    if (Product.length > 0) {
        $("#fillcartitem").html('');
        $("#total").html('');

        var bt = document.getElementById('btn-sbt');
        var bt1 = document.getElementById('btn-cnl');

        bt.disabled = false;
        bt1.disabled = false;

        var t = document.getElementById('btn-sbt1');
        var t1 = document.getElementById('btn-cnl1');

        t.disabled = false;
        t1.disabled = false;


        var total = 0.0;
        var Fill = '';
        Fill =
            '<thead>' +
            '<tr>' +
            '<th>ITEM</th>' +
            '<th>RATE</th>' +
            '<th class="text-center">QTY</th>' +
            '<th></th>' +
            '</tr>' +
            '</thead>' +
            '<tbody>' +
            '<tr>';
        for (var i = 0; i < Product.length; i++) {
            //total += data.lineCollection[i].Product.Price * data.lineCollection[i].Product.Qnautity;
            total += Number(Product[i].Price) * Number(Product[i].Quantity);
            Fill += '<td style="min-width: 135px;max-width: 135px;">';
            Fill += '<h4>' + Product[i].Item_Description + '</h4>';

            Fill += '</td>' +
                '<td style="min-width: 40px;max-width: 40px;">' + Product[i].Price + '</td>' +
                '<td style="width:35%;">' +
                '<div class="input-group">' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number" href="#1" onclick="MinusQtyCookie(this.id);">' +
                '  -' +
                '</button>' +
                '</span>' +
                '<div class="start" >' +
                '<input type="number" style="text-align: center;" required="" name="QuranTilawat_Sipara" class="form-control input-number ng-pristine ng-untouched ng-valid ng-valid-min ng-valid-required" value="' + Product[i].Quantity + '" min="1" disabled="disabled">' +
                '</div>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1" onclick="AddQtyCookie(this.id);">' +
                '  +' +
                '</button>' +
                '</span>' +
                '</div>' +
                '</td>' +
                '<td>' +
                '<span class="input-group-btn">' +
                '<button id="' + Product[i].Sr + '" type="button" class="btn btn-default btn-number"  href="#1"  onclick="RemoveFromCookie(this.id,\'' + Product[i].ComboID + '\');">' +
                'x' +
                '</button>' +
                '</span>' +
                '</td>' +
                '</tr>';
        }
        Fill += '<tbody>';
        $("#fillcartitem").append(Fill);
        $("#total").append(total);
        $('#navmenu').fadeToggle();
        $('#menu-details').fadeToggle(500); $('#cart-details').fadeToggle(500);
        $('#menu-list').fadeToggle(1000);
        $('#cart_show').fadeToggle();
        $('#back_menu').fadeToggle();
        $('#back_menu1').fadeToggle();
    }
    else {
        $('#CartPopup').css('display', 'block');
    }
}

function clearcart() {
    swal({
        title: "Are you sure?",
        text: "Once Clear, you will not be able to recover this Order!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                swal("Poof! Your Order has been Cancle!", {
                    icsson: "success",
                });
                //--------------
                setCookie("Product", "[]", 7);

                $("#fillcartitem").html('');
                $("#fillcartitem").append('');
                $("#total").html('');
                $("#total").append('00.00');
                $('#cartcount').html('');
                $("#cartcount").append('<button type="button" class="button-78" id="open_cart" style="float:right;" href="#1" onclick="ShowCartItem()"><i class="icon-food"></i> : 0 </button>');

                $('#menu-details').fadeToggle(500);
                $('#cart-details').fadeToggle(500);
                $('#menu-list').fadeToggle(1000);
                $('#cart_show').fadeToggle();
                $('#back_menu').fadeToggle();
                $('#back_menu1').fadeToggle();
                $('#navmenu').fadeToggle();
                //---------------
            } else {
                swal("Your Order is safe!");
            }
        });

}