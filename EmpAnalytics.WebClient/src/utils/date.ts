export function formatDate(value: string | number | Date) {
  const date = new Date(value);

  const dateParts = {
    month: `${date.getMonth() + 1}`.padStart(2, '0'),  /* Month range is [0,11]. Also 2 digits in numbers. */
    day: `${date.getDate()}`.padStart(2, '0'),
    year: date.getFullYear(),
    hours: `${date.getHours()}`.padStart(2, '0'),
    minutes: `${date.getMinutes()}`.padStart(2, '0'),
    seconds: `${date.getSeconds()}`.padStart(2, '0'),
  };

  return `${dateParts.year}-${dateParts.month}-${dateParts.day} ${dateParts.hours}:${dateParts.minutes}`;
}