import { Component } from '@angular/core';
import { WeatherComponent } from '../weather/weather.component';

@Component({
  selector: 'app-main',
  imports: [WeatherComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {

  title = 'WeatherFrontend';
}
