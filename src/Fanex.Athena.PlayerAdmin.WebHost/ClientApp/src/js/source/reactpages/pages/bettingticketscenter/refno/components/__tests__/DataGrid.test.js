import React from 'react';
import DataGrid from '../DataGrid';
import ShallowRenderer from 'react-test-renderer/shallow';

describe('DataGrid ', () => {
    it('render correctly', () => {
        const renderer = new ShallowRenderer();

        const result = renderer.render(<DataGrid />);

        expect(result).toMatchSnapshot();
    });
});