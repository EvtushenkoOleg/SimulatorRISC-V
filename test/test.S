.data
N:
.word 0x8884
M:
.word 7

.text

main:
li x29, -4
//slti x28, x29, 2
//sltiu x28, x29, 3
//xori x28, x29, 2
//andi x28, x29, 4
//ori x28, x28, 0xF
//slli x28, x29, 2
li x28, 5
//srai x28, x29, 1
add x30, x29, x28

li x29, 25
srli x28, x29, 4
sub x30, x29, x28
li x28, 4
sll x30, x29, x28
li x28, 1
srl x30, x29, x28
li x28, 2
sra x30, x29, x28
li x28, 30
slt x30, x29, x28
li x28, -5
li x29, 4
sltu x30, x29, x28
li x28, 5
xor x30, x29, x28
or x30, x29, x28
and x30, x29, x28

//jal exit
lb x28, N
beq x28, x29, exit
lh x28, N
//bne x28, x29, exit
lbu x28, N
bltu x28, x29, exit
lhu x28, N
//bgeu x28, x29, exit
sb x28, 8(gp)
sh x28, 292(gp)

exit:
addi a0, x0, 290
ret
