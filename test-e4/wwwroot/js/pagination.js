/* Pagination Function*/

function getPageList(totalPages, page, maxLength){
    function range(start, end){
        return Array.from(Array(end - start + 1), (_, i) => i + start);
    }

    var sideWidth = maxLength < 9 ? 1 : 2;
    var leftWidth = (maxLength - sideWidth * 2 - 3) >> 1;
    var rightWidth = (maxLength - sideWidth * 2 - 3) >> 1;

    if (totalPages <= maxLength) {
        return range(1, totalPages);
    }

    if (page <= maxLength - sideWidth - 1 - rightWidth ){
        return range(1, maxLength - sideWidth - 1).concat(0, range(totalPages - sideWidth + 1, totalPages));
    }

    if (page >= totalPages - sideWidth - 1 - rightWidth) {
        return range(1, sideWidth).concat(0, range(totalPages - sideWidth - 1 - rightWidth - leftWidth, totalPages));
    }
    
    return range(1, sideWidth).concat(0, range(page - leftWidth, page + rightWidth), 0, range(totalPages - sideWidth + 1, totalPages));
}

function setupPagination(productSelector, paginationSelector) {
    var $products = $(productSelector + ' .pro');
    var numberOfItems = $products.length;
    var limitPerPage = 5;
    var totalPages = Math.ceil(numberOfItems / limitPerPage);
    var paginationSize = 7;
    var currentPage = 1;

    function showPage(whichPage){
        if(whichPage < 1 || whichPage > totalPages) return false;
        currentPage = whichPage;

        $products.hide().slice((currentPage - 1) * limitPerPage, currentPage * limitPerPage).show();

        var $pagination = $(paginationSelector);
        $pagination.find('li.pageItem').not('.prevPage, .nextPage').remove();

        getPageList(totalPages, currentPage, paginationSize).forEach(item => {
            $('<li>').addClass('pageItem').addClass(item ? 'currentPage' : 'dots')
            .toggleClass('active', item === currentPage)
            .append($('<a>').addClass('pageLink').attr({href: 'javascript:void(0)'}).text(item || '...'))
            .insertBefore($pagination.find('.nextPage'));
        });

        $pagination.find('.prevPage').toggleClass('disable', currentPage === 1);
        $pagination.find('.nextPage').toggleClass('disable', currentPage === totalPages);

        return true;
    }

    var $pagination = $(paginationSelector);
    $pagination.empty().append(
        $('<li>').addClass('pageItem prevPage').append($('<a>').addClass('pageLink').attr({href: 'javascript:void(0)'}).text('Prev')),
        $('<li>').addClass('pageItem nextPage').append($('<a>').addClass('pageLink').attr({href: 'javascript:void(0)'}).text('Next'))
    );

    showPage(1);

    $pagination.off('click').on('click', 'li.pageItem', function(e){
    e.preventDefault();

    var $li = $(this);

    if ($li.hasClass('disable') || $li.hasClass('active') || $li.hasClass('dots')) {
        // Do nothing for disabled, active, or dots items
        return;
    }

    if ($li.hasClass('nextPage')) {
        showPage(currentPage + 1);
    } else if ($li.hasClass('prevPage')) {
        showPage(currentPage - 1);
    } else {
        var pageNum = parseInt($li.text());
        if (!isNaN(pageNum)) {
            showPage(pageNum);
        }
    }
});

}

// Call for each container + pagination
$(function() {
    setupPagination('.shopContainer', '.shopPagination');
});