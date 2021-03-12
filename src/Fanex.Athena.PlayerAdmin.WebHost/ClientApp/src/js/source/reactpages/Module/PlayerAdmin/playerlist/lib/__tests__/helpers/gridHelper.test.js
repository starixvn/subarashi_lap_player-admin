import GridHelper from '../../helpers/gridHelper';
import { fields } from '../../constants';
import React from 'react';
import { shallow } from 'enzyme';

describe('GridHelper', () => {
    let windowHeight;

    beforeAll(() => {
        windowHeight = window.innerHeight;
    })

    afterAll(() => {
        window.innerHeight = windowHeight;
    })

    const displayedColumn = Object.keys(fields).reduce((acc, currentItem) => {
        acc[currentItem] = true;

        return acc;
    }, {});

    it('calculateTopPosition - return correct top position', () => {
        window.innerHeight = 1000;

        const runTest = (dropdownHeight, currentTop, expectedTopPosition) => {
            const result = GridHelper.calculateTopPosition(dropdownHeight, currentTop);

            expect(result).toBe(expectedTopPosition);
        }

        runTest(40, 100, 100);
        runTest(40, 1000, 960);
    })

    it('renderHeader -  match snapshot', () => {
        const headerContent = GridHelper.renderHeader(displayedColumn);
        const wrapper = shallow(<div>{headerContent}</div>);

        expect(wrapper).toMatchSnapshot();
    })
})