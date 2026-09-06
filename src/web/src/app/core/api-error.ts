import { HttpErrorResponse } from '@angular/common/http';

export class ApiError extends Error {
  constructor(
    readonly status: number,
    message: string,
    readonly fieldErrors: Record<string, string[]> = {},
  ) {
    super(message);
  }
}

export function toApiError(failure: unknown): ApiError {
  if (!(failure instanceof HttpErrorResponse))
    return new ApiError(0, 'Something went wrong before the request was sent.');

  if (failure.status === 0)
    return new ApiError(0, 'The API did not answer. Check that it is running.');

  const problem = failure.error ?? {};
  const fieldErrors: Record<string, string[]> = problem.errors ?? {};
  const reported = Object.values(fieldErrors).flat();

  const message = reported[0] ?? problem.detail ?? problem.title ?? messageFor(failure.status);

  return new ApiError(failure.status, message, fieldErrors);
}

function messageFor(status: number): string {
  const known: Record<number, string> = {
    401: 'That email and password do not match the demo account.',
    404: 'That record no longer exists.',
  };

  return known[status] ?? 'The API refused the request.';
}
