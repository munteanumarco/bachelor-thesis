enum Status {
  NEW = 1,
  IN_PROGRESS = 2,
  RESOLVED = 3,
}

const statusNames: { [key in Status]: string } = {
  [Status.NEW]: 'New',
  [Status.IN_PROGRESS]: 'In progress',
  [Status.RESOLVED]: 'Resolved',
};

const getStatusName = (status: Status): string => statusNames[status];

export { Status, getStatusName };
