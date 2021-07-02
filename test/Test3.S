.data
N:
.word 5
M:
.word 5
matrix:
.word 6, 1, 1, 1, 1
.word 2, 7, 2, 2, 2
.word 3, 3, 8, 3, 3
.word 4, 4, 4, 9, 4
.word 5, 5, 5, 5, 10
result:
.word 0

.text

main:
la a2, N
la a3, M
la a4, matrix
la a5, result
call process
call print
call exit

print:
addi a0, x0, 1 # print_int ecall
lw a1, 0(a5)
ecall

exit:
addi a0, x0, 10
ecall

process:
lw t1, 0(a2) # N
lw t2, 0(a3) # M

addi t4, x0, 0 # row pointer
addi t5, x0, 0 # result
addi t3, x0, 0 # column pointer
cycle:

bne t4, t3, next
lw t6, 0(a4) # load value from array
add t5, t5, t6 # count sum
next:
addi a4,a4,4
addi t3, t3, 1
blt t3, t1, cycle
addi t4, t4, 1
addi t3, x0, 0 # column pointer
blt t4, t2, cycle
sw t5, 0(a5) # save result to resulting array
ret