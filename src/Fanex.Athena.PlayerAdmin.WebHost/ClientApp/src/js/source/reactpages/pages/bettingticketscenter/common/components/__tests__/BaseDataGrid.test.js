import React from 'react';
import { BaseDataGrid } from '../BaseDataGrid';
import ShallowRenderer from 'react-test-renderer/shallow';
import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';
import BetListDataGrid from '@jsroot/components/betlist/BetListDataGrid';
import AppData from '@jsroot/lib/AppData';

jest.mock('@jsroot/lib/AppData');

describe('BaseDataGrid ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
    })
    afterEach(() => {
        jest.resetAllMocks();
    })

    it('no data - return null', () => {
        const props = { toggleLoader: jest.fn(), requestParam: {} };
        
        const shallowRenderer = new ShallowRenderer();
        const result = shallowRenderer.render(<BaseDataGrid {...props} />);

        expect(result).toBe(null);
    });

    it('has data - map snapshot', () => {
        const props = {
            toggleLoader: jest.fn(),
            requestParam: { PageIndex: 1, PageSize: 50 },
            reportTitle: 'title',
            gridData: { Data: [{}] }
        };

        const shallowRenderer = new ShallowRenderer();
        const result = shallowRenderer.render(<BaseDataGrid {...props} />);

        expect(result).toMatchSnapshot();
    });

    describe('BetListDataGrid', () => {
        let betListGridComponent;
        const props = {
            toggleLoader: jest.fn(),
            requestParam: {},
            gridData: { Data: [{}] },
            getGridData: jest.fn(),
            setRequestParam: jest.fn(),
            dataUrl: 'url'
        };

        beforeAll(() => {         
            const shallowRenderer = new ShallowRenderer();
            shallowRenderer.render(<BaseDataGrid {...props} />);
            const result = shallowRenderer.getRenderOutput();

            betListGridComponent = result.props.children.find(x => x.type === BetListDataGrid);
        });

        it.each([['LicenseeWL', false, 'LicenseeWL/Comm'], ['Index', true, '#'], ['WinLoss', false, 'WinLoss'], ['WinLoss', true, 'WinLoss/Comm']])(
            'getHeaderName - return correct data',
            (column, isAllowedCustomerComm, expected) => {
                Object.assign(AppData, { isAllowedCustomerComm: isAllowedCustomerComm });

                const actual = betListGridComponent.props.getHeaderNameFunc(column);

                expect(actual).toBe(expected);
            }
        );

        it('onPaging - call getData with correct parameter', () => {
            const index = 1, size = 2;
            const expectedParam = { PageIndex: 1, PageSize: 2 };

            betListGridComponent.props.onPaging(index, size);

            expect(props.toggleLoader).toHaveBeenCalledWith(true);
            expect(props.getGridData).toHaveBeenCalledWith(props.dataUrl, expectedParam);
            expect(props.setRequestParam).toHaveBeenCalledWith(expectedParam);
        });

        it.each([['WinLoss', 'asc', 'WinLost'], ['Stake', 'desc', 'Stake']])(
            'onSort - call getData with correct parameter',
            (sortBy, sortOrder, expectedSortBy) => {
                const expectedParam = { SortBy: expectedSortBy, SortOrder: sortOrder };

                betListGridComponent.props.onSort(sortBy, sortOrder);

                expect(props.toggleLoader).toHaveBeenCalledWith(true);
                expect(props.getGridData).toHaveBeenCalledWith(props.dataUrl, expectedParam);
                expect(props.setRequestParam).toHaveBeenCalledWith(expectedParam);
            }
        );

        it('afterLoad - call props.toggleLoader', () => {
            betListGridComponent.props.afterLoad();

            expect(props.toggleLoader).toHaveBeenCalledWith(false);
        });

        it('getResourceFunc - call LangHelper.getSingleResource', () => {
            const key = 'test';

            betListGridComponent.props.getResourceFunc(key);

            expect(LangHelper.getSingleResource).toHaveBeenCalledWith(key);
        });
    });
})