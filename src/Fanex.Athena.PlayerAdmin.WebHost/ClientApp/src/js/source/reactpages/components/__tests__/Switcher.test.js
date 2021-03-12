import React from 'react';
import renderer from 'react-test-renderer';

import Switcher from '../Switcher';

describe('Switcher', () => {
    it('render - should match with snapshot', () => {
        const component = renderer.create(<Switcher className="custom-switcher" />);

        const result = component.toJSON();

        expect(result).toMatchSnapshot();
    })
})