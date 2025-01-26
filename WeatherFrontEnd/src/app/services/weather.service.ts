import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";


@Injectable()
export class WeatherService {

    // private apiHost: string = "http://localhost:5009/api";
    private apiHost: string = "/api";



    constructor(private http: HttpClient) { }

    getCityCoordinates(cityName: string): Observable<any> {
        const url = `${this.apiHost}/city/${cityName}`;
        return this.http.get<any>(url);
    }

    getCurrentWeather(cityName: string, lat: number, lon: number): Observable<any> {
        const url = `${this.apiHost}/currentweather/${cityName}/${lat}/${lon}`;
        return this.http.get<any>(url);
    }

    getWeatherForecast(cityName: string, lat: number, lon: number): Observable<any> {
        const url = `${this.apiHost}/weatherforecast/${cityName}/${lat}/${lon}`;
        return this.http.get<any>(url);
    }
}
