import React from 'react';
import { PageLayout } from '../PageLayout';
import ShallowRenderer from 'react-test-renderer/shallow';
import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';
import BetListLayout from '@jsroot/layouts/betlist/BetListLayout';

describe('PageLayout ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Site.resolveClientUrl = jest.fn((url) => url);
        Site.getRootUrl = jest.fn(() => 'root');
    })
    afterEach(() => {
        jest.resetAllMocks();
    })

    it('render with correct active tab', () => {
        const props = { toggleLoader: jest.fn(), children: [], selectedTabIndex: 0, isConvertAll: true, selectedBaseCurrency: 'eur'};
        
        const shallowRenderer = new ShallowRenderer();
        shallowRenderer.render(<PageLayout {...props} />);
        const result = shallowRenderer.getRenderOutput();
        const betListComponent = result.props.children;

        expect(betListComponent.type).toBe(BetListLayout);
        expect(result).toMatchSnapshot();
    });

    it('show loading before showing', () => {
        const props = { toggleLoader: jest.fn(), children: [], selectedTabIndex: 0, isConvertAll: true, selectedBaseCurrency: 'eur' };

        const shallowRenderer = new ShallowRenderer();
        shallowRenderer.render(<PageLayout {...props} />);
        const result = shallowRenderer.getRenderOutput();

        const betListComponent = result.props.children;
        betListComponent.props.beforeShowFunc();

        expect(props.toggleLoader).toHaveBeenCalledWith(true);
    });

    it('hide loading after showing', () => {
        const props = { toggleLoader: jest.fn(), children: [], selectedTabIndex: 0, isConvertAll: true, selectedBaseCurrency: 'eur' };

        const shallowRenderer = new ShallowRenderer();
        shallowRenderer.render(<PageLayout {...props} />);
        const result = shallowRenderer.getRenderOutput();

        const betListComponent = result.props.children;
        betListComponent.props.afterShowFunc();

        expect(props.toggleLoader).toHaveBeenCalledWith(false);
    });

    it('getResource - LangHelper.getSingleResource is called', () => {
        const props = { toggleLoader: jest.fn(), children: [], selectedTabIndex: 0, isConvertAll: true, selectedBaseCurrency: 'eur' };
        const key = 'Resource';

        const shallowRenderer = new ShallowRenderer();
        shallowRenderer.render(<PageLayout {...props} />);
        const result = shallowRenderer.getRenderOutput();

        const betListComponent = result.props.children;
        betListComponent.props.getResourceFunc(key);

        expect(LangHelper.getSingleResource).toHaveBeenCalledWith(key);
    });
})