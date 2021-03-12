import Timer from '../timer';

describe('Timer', () => {
    it('debounce - setTimeout should be called with correct function', () => {
        jest.useFakeTimers();
        const func = jest.fn();

        const debounceFunction = Timer.debounce(func);

        window.clearTimeout = jest.fn();
        window.setTimeout = jest.fn();

        debounceFunction();

        expect(window.clearTimeout).toHaveBeenCalled();
        expect(window.setTimeout).toHaveBeenCalled();
    })

    it('debounce - setTimeout should be called with correct parameters', () => {
        jest.useFakeTimers();
        const func = jest.fn();
        func.apply = jest.fn();
        const timeToWait = 300;

        const debounceFunction = Timer.debounce(func, timeToWait);

        let callback = null;
        let waitTime = null;
        window.clearTimeout = jest.fn();
        window.setTimeout = jest.fn((cb, wait) => {
            callback = cb;
            waitTime = wait;
        });

        debounceFunction();
        callback();

        expect(func.apply).toHaveBeenCalled();
        expect(waitTime).toBe(timeToWait);
    })
})