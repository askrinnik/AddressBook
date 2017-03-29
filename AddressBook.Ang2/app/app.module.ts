import { Component, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { PomodoroTimerComponent } from './app.component';


// Main module, bootstrapping PomodoroTimerComponent as root component
@NgModule({
  imports: [BrowserModule],
  declarations: [PomodoroTimerComponent],
  bootstrap: [PomodoroTimerComponent]
})
export class AppModule { }