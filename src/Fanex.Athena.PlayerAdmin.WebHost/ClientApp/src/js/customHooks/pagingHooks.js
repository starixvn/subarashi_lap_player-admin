import { useState } from 'react';

const defaultPagingOption = {
    PageIndex: 1,
    PageSize: 50,
    TotalItem: 0,
    PageList: [50, 100, 200, 300, 400, 500, 1000, 2000]
};

export const usePagingOption = (initialValue = defaultPagingOption) => {
    const [pagingOption, setPagingOption] = useState({ ...initialValue });
    const handleChangePaging = (pageIndex, pageSize) => {
        setPagingOption({ ...pagingOption, PageSize: pageSize, PageIndex: pageIndex });
    }

    return {
        pagingOption,
        setPagingOption,
        onChangePaging: handleChangePaging
    }
}