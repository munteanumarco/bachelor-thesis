enum Severity {
  LOW = 1,
  MEDIUM = 2,
  HIGH = 3,
  CRITICAL = 4,
}

const severityNames: { [key in Severity]: string } = {
  [Severity.LOW]: 'Low',
  [Severity.MEDIUM]: 'Medium',
  [Severity.HIGH]: 'High',
  [Severity.CRITICAL]: 'Critical',
};

export { Severity, severityNames };
