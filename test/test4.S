.data
N:
.word 0x8884
M:
.word 7

.text

main:
li x29, 25
srli x28, x29, 4
sub x30, x29, x28
sll x28, x29, x30
srl x30, x29, x28
sra x28, x29, x30
slt x30, x29, x28