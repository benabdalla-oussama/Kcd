import http from 'k6/http';
import { check, sleep } from 'k6';
import { group } from 'k6';

export let options = {
    vus: 10,
    duration: '1m',
};

const BASE_URL = 'http://localhost:5160';
const USERNAME = 'admin@admin.com';
const PASSWORD = 'Azerty123456789!.'; 

export default function () {
    group('User Login and Get Token', function () {
        let loginRes = http.post(`${BASE_URL}/api/auth/login`, JSON.stringify({
            email: USERNAME,
            password: PASSWORD,
        }), {
            headers: { 'Content-Type': 'application/json' },
        });

        check(loginRes, {
            'login was successful': (r) => r.status === 200,
            'token is present': (r) => r.json('token') !== '',
        });

        let authToken = loginRes.json('token');

        sleep(1);

        group('Access Applications Endpoint', function () {
            let applicationsRes = http.get(`${BASE_URL}/api/applications`, {
                headers: {
                    Authorization: `Bearer ${authToken}`,
                },
            });

            check(applicationsRes, {
                'applications request was successful': (r) => r.status === 200
            });
        });
    });

    sleep(1);
}
