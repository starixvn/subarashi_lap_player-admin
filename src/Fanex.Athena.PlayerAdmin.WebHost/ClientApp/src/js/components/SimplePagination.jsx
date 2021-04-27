import React from 'react';
import PropTypes from 'prop-types';

const MIN_PAGE_INDEX = 1;
const SimplePagination = (props) => {
    const gotoPage = page => {
        const { totalPage, onPageChanged } = props;
        const currentPage = Math.max(MIN_PAGE_INDEX, Math.min(page, totalPage));

        onPageChanged && onPageChanged(currentPage);
    }

    const handleMovePrev = evt => {
        evt.preventDefault();

        const { pageIndex, totalPage } = props;
        const nextPageIndex = pageIndex - 1;

        if (nextPageIndex >= 1 && nextPageIndex <= totalPage) {
            gotoPage(nextPageIndex);
        }
    }

    const handleMoveNext = evt => {
        evt.preventDefault();
        const { pageIndex, totalPage } = props;
        const nextPageIndex = pageIndex + 1;

        if (nextPageIndex >= 1 && nextPageIndex <= totalPage) {
            gotoPage(nextPageIndex);
        }
    }

    const { pageIndex, totalPage } = props;

    return (
        <div className="simple-pagingation-container">
            <button type="button" className="btn-move btn-prev" disabled={pageIndex <= MIN_PAGE_INDEX} onClick={handleMovePrev}>Prev</button>
            <span className="page-info">{pageIndex}</span>
            <button type="button" className="btn-move btn-next" disabled={pageIndex >= totalPage} onClick={handleMoveNext}>Next</button>
        </div>
    )
}

SimplePagination.propTypes = {
    pageIndex: PropTypes.number,
    totalPage: PropTypes.number,
    onPageChanged: PropTypes.func
}

SimplePagination.defaultProps = {
    pageIndex: 0,
    totalPage: 0
}

export default SimplePagination;