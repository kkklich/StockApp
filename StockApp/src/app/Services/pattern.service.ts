import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class PatternService {

    constructor() {
    }


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

}