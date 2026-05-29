import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import {plainToInstance} from 'class-transformer';
import { Collection } from '../models/Collection';
import { ApiEndPoints } from '../utils/api.endpoints';

@Injectable()
export class ApiService {

  constructor(private http: HttpClient) {
  }

  public getCollections(): Observable<Collection[]> {
    return this.http.get<Collection[]>(ApiEndPoints.Collections.collectionsResource).pipe(map((usr) => plainToInstance(Collection, usr)));
  }
}
