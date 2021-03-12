import React from 'react';
import ShallowRenderer from 'react-test-renderer/shallow';

import { AppContainer } from '../AppContainer';

describe('AppContainer', () => {
    it('render - match with snapshot', () => {
        const runTest = (isLoading) => {
            const renderer = new ShallowRenderer();
            const result = renderer.render(<AppContainer isLoading={isLoading} />);

            expect(result).toMatchSnapshot();
        }

        runTest(true);
        runTest(false);
    })
})