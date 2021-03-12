import React from 'react';
import renderer from 'react-test-renderer';
import CollapsiblePanel from '../CollapsiblePanel';

describe('CollapsiblePanel', () => {
    it('constructor - initial state should be correct', () => {
        const instance = new CollapsiblePanel();
        const expectedState = {
            isCollapsed: false
        }

        expect(instance.state).toEqual(expectedState);
    })

    describe('onClick', () => {
        it('setState should call with correct new state and callback', () => {
            let changedState = null;
            let callback = null;
            const prop = {
                onCollapse: jest.fn(),
                onExpand: jest.fn()
            }
            const instance = new CollapsiblePanel(prop);
            instance.setState = jest.fn((newState, cb) => {
                changedState = newState;
                callback = cb;
            });

            window.dispatchEvent = jest.fn();

            const expectedState = {
                isCollapsed: true
            }

            instance.onClick();
            callback();

            expect(changedState).toEqual(expectedState);
            expect(window.dispatchEvent).toHaveBeenCalled();
        })
    })

    describe('render', () => {
        it('isCollapsed = true - should render arrowStyle correctly', () => {
            const component = renderer.create(
                <CollapsiblePanel>
                    <div>Filter item 1</div>
                    <div>Filter item 2</div>
                </CollapsiblePanel>
            );

            expect(component.toJSON()).toMatchSnapshot();
        })
    })
})