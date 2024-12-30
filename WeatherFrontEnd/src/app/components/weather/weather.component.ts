import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { WeatherService } from '../../services/weather.service';

@Component({
  selector: 'app-weather',
  imports: [CommonModule],
  templateUrl: './weather.component.html',
  styleUrl: './weather.component.css',
  providers: [WeatherService],
  standalone: true
})

export class WeatherComponent {

  cityName: string = '';
  lat: number = 0;
  lon: number = 0;
  cities: any[] = [];
  currentWeather: any = null;
  weatherForecast: any[] = [];
  errorMessage: string = '';

  constructor(private weatherService: WeatherService) { }


}
