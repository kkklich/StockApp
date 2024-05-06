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
    public weightedMovingAverage(data: number[], length: number): number[] {
        const result: number[] = [];
        let sum = 0;
        let weights = 0;

        for (let i = 0; i < data.length; i++) {
            if (i >= length) {
                sum -= data[i - length];
                weights--;
            }
            sum += data[i];
            weights++;
            result.push(sum / weights);
        }

        return result;
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


    //wskażnik impetu RSI
    //oscylator stochastyczny
    //wskażnik zmienności ATR
    //wskażnik ADX




}