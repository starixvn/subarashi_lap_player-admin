import React from 'react';
import { shallow } from 'enzyme';
import { UserNameCell } from '../../grid/UserNameCell';

describe('UserNameCell', () => {
    const defaultProps = {
        dataItem: {
            LicenseeCustId: 11,
            IbcCustId: 1,
            UserName: 'test',
            CurrencyId: 20,
            Currency: 'UUS',
            Status: 0,
            IsDeposit: true,
            IsArchived: false,
            OddsGroup: 'A',
            SpecialLevel: 0
        },
        setSelectedPlayer: jest.fn(),
        setCurrentActivePlayer: jest.fn(),
        setContextMenu: jest.fn(),
        setSelectedTabName: jest.fn(),
        getTabList: jest.fn(() => {
            return [
                { key: 'note', name: 'Note', tabName: 'Note' },
                { key: 'password', name: 'Password', tabName: 'Setting' }
            ]
        })
    }

    it('should match snapshot', () => {
        const wrapper = shallow(<UserNameCell {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    })

    it('click username link - setSelectedPlayer should be called with correct parameter', () => {
        const props = {
            ...defaultProps,
            setSelectedPlayer: jest.fn()
        }
        const wrapper = shallow(<UserNameCell {...props} />);
        const { IbcCustId, LicenseeCustId, UserName, Currency, IsDeposit, IsArchived, CurrencyId } = props.dataItem;
        const expectedParam = {
            ibcCustId: IbcCustId,
            licenseeCustId: LicenseeCustId,
            userName: UserName,
            currency: Currency,
            isDeposit: IsDeposit,
            isArchived: IsArchived,
            currencyId: CurrencyId
        }

        wrapper.find('.username-link').simulate('click', { preventDefault: jest.fn() });

        expect(props.setSelectedPlayer).toHaveBeenCalledWith(expectedParam);
    })

    it('onMouseEnter event with menu icon - setCurrentActivePlayer should be called with correct parameter', () => {
        const props = {
            ...defaultProps,
            contextMenu: { isOpen: false },
            setCurrentActivePlayer: jest.fn()
        }

        const wrapper = shallow(<UserNameCell {...props} />);
        const { IbcCustId, UserName, Currency, Status, OddsGroup, SpecialLevel, IsDeposit, IsArchived, LicenseeCustId, CurrencyId } = props.dataItem;
        const expectedParam = {
            ibcCustId: IbcCustId,
            licenseeCustId: LicenseeCustId,
            userName: UserName,
            currency: Currency,
            status: Status,
            oddsGroup: OddsGroup,
            specialLevel: SpecialLevel,
            isDeposit: IsDeposit,
            isArchived: IsArchived,
            currencyId: CurrencyId
        }

        const event = {
            preventDefault: jest.fn(),
            target: {
                getBoundingClientRect: jest.fn(() => ({ top: 100, left: 100 }))
            }
        }

        wrapper.find('.cell-menu').simulate('mouseenter', event);

        expect(props.setContextMenu).toHaveBeenCalled();
        expect(props.setCurrentActivePlayer).toHaveBeenCalledWith(expectedParam);
    })

    it('onMouseLeave event with menu icon - handleLeaveContextMenu should be called', () => {
        const props = {
            ...defaultProps,
            contextMenu: {isOpen: true},
            setContextMenu: jest.fn()
        }
        const wrapper = shallow(<UserNameCell {...props} />);
        const event = {
            preventDefault: jest.fn()
        }

        wrapper.find('.cell-menu').simulate('mouseleave', event);

        expect(props.setContextMenu).toHaveBeenCalledWith({isOpen: false});
    })
})