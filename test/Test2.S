.data
N:
.word 4
M:
.word 3
matrix:
.word 1, 2, 3
.word 9, 5, 2 
.word 3, 10, 3
.word 3, 8, 3

.text

main:
lw t0, N
addi sp, sp, -12
sw t0, 8(sp)
lw t0, M
sw t0, 4(sp)
la t0, matrix
sw t0, 0(sp)
call process
call res
call exit

res:
addi a0, x0, 1
lw a1, 4 (sp)
ecall

addi a0, x0, 11 # print_char ecall
addi a1, x0, 32
ecall

addi a0, x0, 1
lw a1, 0 (sp)
addi sp,sp, 8
ecall

ret

exit:
addi a0, x0, 10
ecall

process:
lw t0, 0(sp) # arr pointer
addi sp,sp, 4
lw t1, 0(sp) # M
addi sp, sp, 4
lw t2, 0(sp) # N
addi t3, x0, 1 # row pointer
addi t4, x0, 0 # row element number
lw t5, 0(t0) # max
addi sp, sp, -8 
sw x0, 0(sp) # Imax
sw x0, 4(sp) # Jmax
addi t0, t0, 4

loop:
lw t6, 0(t0) # load value from array
bge t5, t6, cont
addi t5, t6, 0
sw t3, 0(sp)
sw t4, 4(sp)

cont:
addi t0, t0, 4 # increment array pointer
addi t3, t3, 1
blt t3, t1, loop
addi t3, x0, 0
addi t4, t4, 1 # increment row element number
blt t4, t2, loop
ret