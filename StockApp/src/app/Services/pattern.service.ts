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


    //wskażnik impetu RSI
    //oscylator stochastyczny
    //wskażnik zmienności ATR
    //wskażnik ADX




}