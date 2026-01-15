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
  import { useCreateBudget, useUpdateBudget } from "@/lib/abp/hooks/useBudgets";
import { toast } from "sonner";

const formSchema = z.object({
  
    year: z.any(),
  
    month: z.any(),
  
    amount: z.any(),
  
    categoryId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface BudgetFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function BudgetForm({
  isOpen,
  onClose,
  initialValues,
}: BudgetFormProps) {
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

  const createMutation = useCreateBudget();
const updateMutation = useUpdateBudget();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Budget updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Budget created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save budget:", error);
    toast.error(error.message || "Failed to save budget");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Budget": "Create Budget" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the budget." : "Fill in the details to create a new budget." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="year" > Year * </Label>

<Input id="year" type = "number" step = "any" {...register("year") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="month" > Month * </Label>

<Input id="month" type = "number" step = "any" {...register("month") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="amount" > Amount * </Label>

<Input id="amount" type = "number" step = "any" {...register("amount") } />

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
