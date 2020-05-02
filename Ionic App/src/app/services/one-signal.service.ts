import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import * as uuid from 'uuid';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

const oneSignalAPIKey = 'M2YzNjcyZmYtMThiOC00NDFhLTg3ODItZGFiOTNlNjZmOTI2';
const oneSignalAppID = '94a25d89-60f4-4c25-a379-96dfc39c61c6';
const oneSignalAPI = 'https://onesignal.com/api/v1/notifications';

@Injectable({
  providedIn: 'root'
})
export class OneSignalService {

  constructor(
    private http: HttpClient
  ) { }

  sendNotification(message: string, redirectUrl: string) {
    // headers
    const headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8"',
      Authorization: 'Basic ' + oneSignalAPIKey
    });
    // body
    const body = {
      app_id: oneSignalAppID,
      included_segments: ['Active Users'],
      data: {},
      external_id: uuid.v4(),
      url: redirectUrl,
      contents: {en: message, es: message}
    };
    // petición
    return new Promise(resolve => {
      this.http.post(`${ oneSignalAPI }`, body, { headers })
                .pipe(
                  catchError(err => of(undefined))
                )
                .subscribe(res => {
                  console.log(res);
                  resolve(res !== undefined);
                });
    });
  }
}
