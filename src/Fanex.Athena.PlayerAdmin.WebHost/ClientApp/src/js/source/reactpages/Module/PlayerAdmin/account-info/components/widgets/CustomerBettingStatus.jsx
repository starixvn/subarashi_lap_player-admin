import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../actions/index.js';
import { bindActionCreators } from 'redux';

import NumberFormat from '@jsroot/lib/numberFormat';

import { Doughnut } from 'react-chartjs-2';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';

const backgroundColorList = ['#36A2EB', '#F15D21', '#97A9B3', '#E8AE1B']
const defaultDataSetItem = {
    data: [],
    backgroundColor: backgroundColorList
}
const defaultChartData = {
    labels: [
        ClientResourceManager.getText('Won'), 
        ClientResourceManager.getText('Lose'), 
        ClientResourceManager.getText('Draw'), 
        ClientResourceManager.getText('Cancelled')
    ],
    datasets: [],
    borderWidth: 2,
    hoverBorderWidth: 10,
}

const legendOpts = {
    display: true,
    position: 'bottom',
    labels: {
        boxWidth: 15,
        usePointStyle: true
    },
    onClick: () => {}
};

const option = {
    animation: {
        onComplete: function () {
            const self = this;
            const chart = this.chart;
            const ctx = chart.ctx;

            const chartSize = Math.min(chart.width, chart.height);
            if (chartSize < 100) {
                return;
            }

            ctx.textAlign = "center";
            ctx.fillStyle = "#fff";
            ctx.font = "1rem sans-serif";
            ctx.save();
            ctx.shadowColor = "black";
            ctx.shadowBlur = 3;
            const summary = self.data.datasets[0].data.reduce((sum, current) => {
                return sum += current;
            }, 0)
            self.data.datasets.forEach((dataset, datasetIndex) => {
                const meta = self.getDatasetMeta(datasetIndex);

                meta.data.forEach((element, index) => {
                    const segmentPercent = dataset.data[index] / summary * 100;
                    const position = element.tooltipPosition();
                    if (element._model.circumference > 0) {
                        const label = segmentPercent < 1 ? "<1" : segmentPercent.toFixed(2);
                        ctx.fillText(label + '%', position.x, position.y);
                    }
                });
            });

            ctx.restore();
        }
    }
}

export const CustomerBettingStatus = (props) => {
    const [data, setData] = useState(null);

    useEffect(() => {
        const fetchData = () => {
            const { ibcCustId } = props;
            const params = { custId: ibcCustId };
            props.getCustomerBettingStatus(params)
                .then(response => {
                    setData(response.data);
                })
                .catch(err => {
                    console.log(err.message);
                });
        }

        fetchData();
    }, []);

    const renderContent = () => {
        const { WonTicketCount, LoseTicketCount, DrawTicketCount, CancelledTicketCount } = data;
        const dataSetItem = {
            ...defaultDataSetItem,
            data: [WonTicketCount, LoseTicketCount, DrawTicketCount, CancelledTicketCount]
        }
        const chartData = {
            ...defaultChartData,
            datasets: [dataSetItem]
        }
        const { Margin, AverageBetSize, Winnings, Loss, NetWinLoss } = data;
        const marginPercent = NumberFormat.round(Margin, 2) * 100;

        return (
            <div className="row">
                <div className="col-12 col-md-7 col-sm-7 col-lg-7 col-xl-7 chart-group">
                    <Doughnut data={chartData} legend={legendOpts} options={option} />
                </div>
                <div className="col-12 col-md-5 col-sm-5 col-lg-5 col-xl-5 statistics-group">
                    <p>{ClientResourceManager.getText('BasedOnSportsbook')}</p>
                    <div className="row">
                        <div className="col-5 col-md-5 col-sm-5 col-lg-5 col-xl-5">{ClientResourceManager.getText('Margin')}:</div>
                        <div className="col-4 col-md-4 col-sm-4 col-lg-4 col-xl-4">{NumberFormat.formatDecimal(marginPercent)}%</div>
                    </div>
                    <div className="row">
                        <div className="col-5 col-md-5 col-sm-5 col-lg-5 col-xl-5">{ClientResourceManager.getText('AverageBetSize')}:</div>
                        <div className="col-4 col-md-4 col-sm-4 col-lg-4 col-xl-4">{NumberFormat.formatDecimal(AverageBetSize)}</div>
                    </div>
                    <div className="row">
                        <div className="col-5 col-md-5 col-sm-5 col-lg-5 col-xl-5">{ClientResourceManager.getText('Winnings')}:</div>
                        <div className="col-4 col-md-4 col-sm-4 col-lg-4 col-xl-4">{NumberFormat.formatDecimal(Winnings)}</div>
                    </div>
                    <div className="row">
                        <div className="col-5 col-md-5 col-sm-5 col-lg-5 col-xl-5">{ClientResourceManager.getText('Loss')}:</div>
                        <div className="col-4 col-md-4 col-sm-4 col-lg-4 col-xl-4">{NumberFormat.formatDecimal(Loss)}</div>
                    </div>
                    <div className="row">
                        <div className="col-5 col-md-5 col-sm-5 col-lg-5 col-xl-5">{ClientResourceManager.getText('NetWinLoss')}:</div>
                        <div className="col-4 col-md-4 col-sm-4 col-lg-4 col-xl-4">{NumberFormat.formatDecimal(NetWinLoss)}</div>
                    </div>
                </div>
            </div>
        )
    }

    return (
        <div className="panel">
            <div className="panel-header">
                {ClientResourceManager.getText('PlayerBettingStatisticsTitle')}
            </div>
            <div className="panel-body">
                {data && data.HasData ? renderContent() : (<p className="center-align">{ClientResourceManager.getText('NoSportsbookData')}</p>)}
            </div>
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { getCustomerBettingStatus } = ActionCreators;
    return bindActionCreators({ getCustomerBettingStatus }, dispatch);
}

function mapStateToProps(state) {
    const { ibcCustId } = state;
    return Object.assign({}, { ibcCustId });
};

export default connect(mapStateToProps, mapDispatchToProps)(CustomerBettingStatus);