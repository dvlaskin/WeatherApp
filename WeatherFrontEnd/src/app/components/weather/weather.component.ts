import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WeatherService } from '../../services/weather.service';

@Component({
  selector: 'app-weather',
  imports: [CommonModule, FormsModule],
  templateUrl: './weather.component.html',
  styleUrl: './weather.component.css',
  providers: [WeatherService],
  standalone: true
})

export class WeatherComponent implements OnInit {

  cityName: string = '';
  cities: any[] = [];
  selectedCity: { cityName: string, lat: number, lon: number } | null = null;
  currentWeather: any = null;
  weatherForecast: any[] = [];
  errorMessage: string = '';

  constructor(private weatherService: WeatherService) { }

  ngOnInit(): void {
    this.loadSelectedCity();
  }

  clearWeather() {
    this.currentWeather = null;
    this.weatherForecast = [];
  }

  searchCity() {
    if (!this.cityName.trim()) {
      return;
    }

    this.clearWeather();

    this.weatherService.getCityCoordinates(this.cityName).subscribe({
      next: (results) => {
        this.cities = results.map((city: any) => ({
          cityName: city.name,
          lat: city.lat,
          lon: city.lon,
          country: city.country,
          state: city.state
        }));

        if (this.cities.length === 0) {
          this.errorMessage = 'City not found';
        } else {
          this.errorMessage = '';
        }
      },
      error: () => {
        this.errorMessage = 'Error occurred while fetching city coordinates';
      },
    });
  }

  selectCity(city: { cityName: string, lat: number, lon: number }) {
    this.selectedCity = city;
    this.saveSelectedCity();
    this.cities = [];
  }

  saveSelectedCity() {
    if (this.selectedCity) {
      localStorage.setItem('selectedCity', JSON.stringify(this.selectedCity));
    }
  }

  loadSelectedCity() {
    if (typeof window !== 'undefined' && window.localStorage) {
      const savedCity = localStorage.getItem('selectedCity');
      if (savedCity) {
        this.selectedCity = JSON.parse(savedCity);
      }
    }
  }

  fetchWeather() {
    if (!this.selectedCity) {
      return;
    }

    this.errorMessage = '';
    const { cityName, lat, lon } = this.selectedCity;

    this.weatherService.getCurrentWeather(cityName, lat, lon).subscribe({
      next: (weather) => {
        this.currentWeather = weather;
      },
      error: () => {
        this.errorMessage = 'Can not load current weather';
      },
    });

    this.weatherService.getWeatherForecast(cityName, lat, lon).subscribe({
      next: (forecast) => {
        this.weatherForecast = forecast;
      },
      error: () => {
        this.errorMessage = 'Can not load weather forecast';
      },
    });
  }

}
