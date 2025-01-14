import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class PatternService {

    constructor() { }

    public calculateAverageRolling(data: number[], windowSize: number): number[] {
        const result: number[] = [];

        for (let i = 0; i <= data.length - windowSize; i++) {
            const window = data.slice(i, i + windowSize);
            const sum = window.reduce((acc, num) => acc + num, 0);
            const average = sum / windowSize;
            result.push(average);
        }
        return result;
    }

    // ważona średnia ruchoma
    public weightedMovingAverage(data: number[], period: number): number[] {
        const wma: number[] = [];
        const weightSum = (period * (period + 1)) / 2; // Sum of weights

        for (let i = period; i <= data.length; i++) {
            let weightedSum = 0;

            for (let j = 0; j < period; j++) {
                weightedSum += data[i - j - 1] * (period - j);
            }

            wma.push(weightedSum / weightSum);
        }
        wma.unshift(...new Array(period).fill(null));

        return wma;
    }

    //wykladnicza srednia ruchoma
    exponentialMovingAverage(data: number[], emaValue: number): number[] {
        let currentValue: number | null = null;
        const alpha = 2 / (emaValue + 1);
        const emaValues: number[] = [];

        for (const value of data) {
            if (currentValue === null) {
                currentValue = value;
            } else {
                currentValue = alpha * value + (1 - alpha) * currentValue;
            }
            emaValues.push(currentValue);
        }
        return emaValues;
    }

    public calculateRSI(prices: number[], period: number = 14): number[] {
        if (prices.length < period) {
            throw new Error("Not enough data points to calculate RSI");
        }

        const rsi: number[] = new Array(prices.length).fill(undefined);

        let gains = 0;
        let losses = 0;

        for (let i = 1; i <= period; i++) {
            const change = prices[i] - prices[i - 1];
            if (change > 0) {
                gains += change;
            } else {
                losses -= change;
            }
        }

        let averageGain = gains / period;
        let averageLoss = losses / period;
        rsi[period] = this.calculateRSIValue(averageGain, averageLoss);

        for (let i = period + 1; i < prices.length; i++) {
            const change = prices[i] - prices[i - 1];
            let gain = 0;
            let loss = 0;

            if (change > 0) {
                gain = change;
            } else {
                loss = -change;
            }

            averageGain = (averageGain * (period - 1) + gain) / period;
            averageLoss = (averageLoss * (period - 1) + loss) / period;

            rsi[i] = this.calculateRSIValue(averageGain, averageLoss);
        }

        return rsi;
    }

    calculateRSIValue(averageGain: number, averageLoss: number): number {
        if (averageLoss === 0) {
            return 100;
        }

        const rs = averageGain / averageLoss;
        return 100 - (100 / (1 + rs));
    }

    private calculateSlope(point1: [number, number], point2: [number, number]): number {
        return (point2[1] - point1[1]) / (point2[0] - point1[0]);
    }

    // Function to detect change points
    public detectChangePoints(data: number[]): number[] {
        const changePoints: number[] = [];
        for (let i = 1; i < data.length - 1; i++) {
            const slope1 = this.calculateSlope([i - 1, data[i - 1]], [i, data[i]]);
            const slope2 = this.calculateSlope([i, data[i]], [i + 1, data[i + 1]]);
            if (slope1 * slope2 < 0) {
                changePoints.push(i);
            }
        }
        return changePoints;
    }

    public showTrend(data: number[], lengthAverge: number): string {
        const firstValue = data[lengthAverge];
        const lastValue = data[data.length - 1];
        const diff = lastValue - firstValue;
        const diffPercentage = ((diff / firstValue) * 100);
        const trendStrength = diffPercentage.toFixed(2);

        if (Math.abs(diffPercentage) < 5)
            return `Boczny (${trendStrength}%)`;

        const trendDirection = diff > 0 ? 'wzrostowy' : 'spadkowy';
        return `${trendDirection} (${trendStrength}%)`;
    }
}