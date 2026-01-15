import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateTransaction, useUpdateTransaction } from "@/lib/abp/hooks/useTransactions";
import { toast } from "sonner";

const formSchema = z.object({
  
    description: z.any(),
  
    amount: z.any(),
  
    type: z.any(),
  
    status: z.any(),
  
    dueDate: z.any(),
  
    paymentDate: z.any(),
  
    clientId: z.any(),
  
    categoryId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface TransactionFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function TransactionForm({
  isOpen,
  onClose,
  initialValues,
}: TransactionFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateTransaction();
const updateMutation = useUpdateTransaction();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Transaction updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Transaction created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save transaction:", error);
    toast.error(error.message || "Failed to save transaction");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Transaction": "Create Transaction" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the transaction." : "Fill in the details to create a new transaction." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="description" > Description * </Label>

<Input id="description" {...register("description") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="amount" > Amount * </Label>

<Input id="amount" type = "number" step = "any" {...register("amount") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="type" > Type * </Label>

<Input id="type" {...register("type") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="status" > Status</Label>

<Input id="status" {...register("status") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dueDate" > DueDate * </Label>

<Input id="dueDate" type = "date" {...register("dueDate") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="paymentDate" > PaymentDate</Label>

<Input id="paymentDate" type = "date" {...register("paymentDate") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="clientId" > ClientId</Label>

<Input id="clientId" {...register("clientId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="categoryId" > CategoryId</Label>

<Input id="categoryId" {...register("categoryId") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
