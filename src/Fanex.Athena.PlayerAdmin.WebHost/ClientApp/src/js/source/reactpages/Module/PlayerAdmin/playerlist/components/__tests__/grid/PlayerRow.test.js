import React from 'react';
import { shallow } from 'enzyme';

import { PlayerRow } from '../../grid/PlayerRow';

describe('PlayerRow', () => {
    const dataItem = {
        LicenseeCustId: 11,
        Status: 'online',
        UserName: 'test1',
        RealName: 'test1 real',
        SystemId: 'abc',
        Currency: 'USD',
        CurrentBalance: 100,
        OutstandingBalance: '200',
        OddsGroup: 'A',
        BaStatus: true,
        SpecialLevel: 1,
        LastLoginIp: '1.1.1.',
        LastTransaction: '06/14/2019',
        Email: 'abc@yahoo.com',
        IsOnline: true,
        IsDeposit: true, 
        IsArchived: false 
    };

    const displayedColumn = {
        CustId: true,
        Status: true,
        Username: true,
        SystemID: true,
        RealName: true,
        Currency: true,
        CurrentBalance: true,
        OutstandingBalance: true,
        OddsGroup: true,
        BAStatus: true,
        SpecialLevel: true,
        LastLoginIp: false,
        LastTransaction: false,
        Email: false
    }

    const defaultProps = {
        dataItem,
        displayedColumn
    }

    it('render - match snapshot', () => {
        const props = {
            ...defaultProps
        }

        const wrapper = shallow(<PlayerRow {...props} />);

        expect(wrapper).toMatchSnapshot();
    })
})