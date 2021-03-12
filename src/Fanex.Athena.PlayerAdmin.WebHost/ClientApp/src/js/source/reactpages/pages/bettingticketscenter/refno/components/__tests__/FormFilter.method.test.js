import React from 'react';
import { FormFilter } from '../FormFilter';
import AppData from '@jsroot/lib/AppData'
import LangHelper from '@jsroot/lib/langHelper';

import { shallow } from 'enzyme';

jest.mock('@jsroot/lib/AppData');

describe('FormFilter ', () => {
    const props = { 
        toggleLoader: jest.fn(), 
        setReportTitle: jest.fn(),
        setRequestParam: jest.fn(),
        getGridData: jest.fn(),
        setSelectedBaseCurrency: jest.fn(),
        setRefno: jest.fn(),
        setUsername: jest.fn(),
        changeIsConvertAll: jest.fn(),
        gridData: [] };

    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Object.assign(AppData, { baseCurrencyList: ['EUR'] });
    });

    it('render correctly', () => {
        const result = shallow(<FormFilter {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('render Base Currency Select List correctly', () => {
        Object.assign(AppData, { isAllowedCustomerComm: true });

        const filter = new FormFilter(props);
        const result = shallow(<div>{filter._renderBaseCurrencySelectList()}</div>);

        expect(result).toMatchSnapshot();
    });
});