//небольшой скрипт для временной демонстрации(!!!) активного пункта меню.
$('.sidebar__menu a').each(function () {
	if (this.pathname == location.pathname) {
		if (!$(this).parent().hasClass('classifier_down')) {
			$(this).parent().addClass('active');
		}
	}
})
$('.classifier a').on('click', function (e) {
	e.preventDefault(); // этот код предотвращает стандартное поведение браузера по клику
	$(this).parent().toggleClass('active');
	$('.classifier_down').toggleClass('show');
});
//шапка
$('.menu-btn').click(function () {
	$(this).toggleClass('menu-btn--active');
	$('.sidebar').toggleClass('sidebar--open')
})
//кастомный "селект"
$('.pseudoselect__input').focus(function () {
	$('.pseudoselect__dropdown').hide();
	$(this).siblings('.pseudoselect__dropdown').show()
})
$(document).click(function (e) {
	if (!$(e.target).closest('.pseudoselect').length) {
		$('.pseudoselect__dropdown').hide();
	}
})
$('.pseudoselect__list li').click(function () {
	var value = $(this).text();
	if ($(this).hasClass('classifer')) {
		$(this).closest('.pseudoselect').find('.id_select').val($(this).data('id'));
	}
	let idCmd = $(this).data('id_cmd');
	let curIdCmd = $(this).closest('.pseudoselect').find('.sel_id_cmd').val();
	$(this).closest('.pseudoselect').find('.sel_id_cmd').val(idCmd);
	if (idCmd != curIdCmd)
		addParams(idCmd);
	$(this).closest('.pseudoselect').find('.pseudoselect__input').val(value);
	$(this).parent().siblings('.pseudoselect__current').text(value);
	$(this).closest('.pseudoselect__dropdown').hide();
})
$('.pseudoselect__input').on('input change', function () {
	if (this.value) {
		$(this).closest('.pseudoselect').find('.pseudoselect__current').text(this.value);
	} else {
		$(this).closest('.pseudoselect').find('.pseudoselect__current').text('Не выбрано');
	}
})
//сортировка таблиц

//прозрачность в контейнере для прокрутки таблиц
function toggleScrollShadow() {
	if (this.clientWidth + this.scrollLeft < this.scrollWidth) {
		this.classList.add('table-wrapper--scrollable')
	} else {
		this.classList.remove('table-wrapper--scrollable');
	}
}
$('.table-wrapper').scroll(toggleScrollShadow);
$(window).resize(function () {
	$('.table-wrapper').each(toggleScrollShadow);
})
//Установка столбцов графика(ов) по высоте в зависимости от значений
function setChartItemsHeight() {
	var values = [];
	$(this).find('.chart__value').each(function () {
		values.push(parseFloat($(this).data('sum')));
	})
	var max = Math.max.apply(null, values);

	for (var i = 0; i < values.length; i++) {
		$(this).find('.chart__item').eq(i).css('min-height', values[i] / max * 100 + '%');
	}
}
$(document).ready(function () {
	$('.chart').each(setChartItemsHeight);
	if ($('.active').hasClass('classifier')) {
		$('.classifier_down').toggleClass('show');
	}
})
$(window).resize(function () {
	$('.chart').each(setChartItemsHeight);
})
//Динамическое добавление форм для параметров команды
function addParams(SelIdCmd) {
	var data = { idCmd: SelIdCmd };
	$.ajax({
		url: '/Cmd/NewCmd',
		type: "post",
		data: data,
		contentType: 'application/x-www-form-urlencoded',
		success: function (html) {
			$('#CmdParams').html(html);
		}
	});
}

///////
$(document).on('click ', function (e) {
    if ($('.nav_menu').css('display') == 'block' && $($(e.target).parent()).attr('class') != 'open_menu_mob' && $(window).width() < 993) {
        $('.nav_menu').fadeOut();
    }
});
$('.open_menu_mob').on('click', function () {
    $('.nav_menu').fadeIn();
    alert("open_menu_mob");
});
$(function () {
    let Order_num = $("#Order_num").val();
    let Order_direction = $("#Order_direction").val();
    $('[data-column=' + Order_num + ']').addClass(Order_direction > 0 ? 'sort-up' : 'sort-down');

    $('.sort').on('click', function () {
        $("#Order_num").val($(this).data('column'));

        if ($(this).attr('class') == "sort") {
            $("#Order_direction").val('-1');
        } else {
            if ($(this).hasClass('sort-up')) {
                $("#Order_direction").val('-1');
            } else {
                $("#Order_direction").val('1');
            }
        }
        $('#filterr').submit();
    });
});
$('.add_item').on('click', function (e) {
    e.preventDefault(); // этот код предотвращает стандартное поведение браузера по клику
    $('.action_name').val($(this).data('action'));
    $('#filterr_action').submit();
});
$('.edit_item').on('click', function () {
    $('.id_action').val($(this).data('id'));
    $('.name_action').val($(this).data('name'));
    $('.action_name').val($(this).data('action'));
    $('#filterr_action').submit();
});
$('.delete_item').on('click', function () {
    $('#filterr_delete').find('.id_action_del').val($(this).data('id'));
    $('.edite_add_block').fadeIn();
});
$('.delete_item_ok').on('click', function () {
    $('#filterr_delete').submit();
    $('.edite_add_block').fadeOut();
});
$('.btn.catalog__btn.gray').on('click', function (e) {
    e.preventDefault(); // этот код предотвращает стандартное поведение браузера по клику
    if ($('.del_formm').css('display') == 'block') {
        $('.edite_add_block').fadeOut();
    } else {
        location.href = window.location.href;
    }
});

$('.page_table').on('click', function () {
    var page = $(this).data('page');
    $('.page_table.active').removeClass('active');
    $(this).addClass('active');
    $('.active_list').removeClass('active_list');
    $('[data-list=' + page + ']').addClass('active_list');
    $("body,html").animate({
        scrollTop: 0
    }, 800);
});