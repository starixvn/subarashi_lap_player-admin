import React from 'react';
import DataGrid from '../DataGrid';
import ShallowRenderer from 'react-test-renderer/shallow';
import LangHelper from '@jsroot/lib/langHelper';

describe('DataGrid ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
    });

    it('render correctly', () => {
        const renderer = new ShallowRenderer();

        const result = renderer.render(<DataGrid />);

        expect(result).toMatchSnapshot();
    });
});