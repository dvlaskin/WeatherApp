import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'WeatherFrontend';
  counter: number = 0;

  onIncrement() {
    this.counter++;
  }

  onDecrement() {
    this.counter--;
  }
}
