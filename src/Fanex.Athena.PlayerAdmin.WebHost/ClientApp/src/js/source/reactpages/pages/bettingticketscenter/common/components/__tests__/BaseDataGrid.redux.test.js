jest.mock('react-redux')
jest.mock('redux')
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../action.js'

describe('connect ', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../BaseDataGrid').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            gridData: [],
            requestParam: 'request',
            reportTitle: 'title'
        }

        const state = {
            gridData: [],
            requestParam: 'request',
            reportTitle: 'title',
            anotherProp: 'Not included'
        }

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);
        const expectedActionCreator = {
            toggleLoader: ActionCreators.toggleLoader,
            getGridData: ActionCreators.getGridData,
            setRequestParam: ActionCreators.setRequestParam
        }

        const dispatch = {};
        mapDispatchToProps(dispatch);

        expect(mockBindAction).toHaveBeenCalledWith(expectedActionCreator, dispatch);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const BaseDataGrid = require('../BaseDataGrid').BaseDataGrid;

        expect(connectMock).toHaveBeenCalledWith(BaseDataGrid);
    })
})