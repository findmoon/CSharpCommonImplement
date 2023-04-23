$source = Get-Content -Raw -Path ".\Calculator.cs"
Add-Type -TypeDefinition "$source"  -Language CSharp

# Call a static method
[Calculator]::Add(4, 3)

# Create an instance and call an instance method
$calculatorObject = New-Object Calculator
$calculatorObject.Multiply(5, 2)

# Create an instance use constructor of params and call an instance method
$calculatorObject1 = New-Object Calculator(6)
$calculatorObject1.BasePlusMultiply(5, 2)
