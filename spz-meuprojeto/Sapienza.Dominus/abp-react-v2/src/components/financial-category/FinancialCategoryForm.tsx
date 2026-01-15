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
  import { useCreateFinancialCategory, useUpdateFinancialCategory } from "@/lib/abp/hooks/useFinancialCategories";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    type: z.any(),
  
    color: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinancialCategoryFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinancialCategoryForm({
  isOpen,
  onClose,
  initialValues,
}: FinancialCategoryFormProps) {
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

  const createMutation = useCreateFinancialCategory();
const updateMutation = useUpdateFinancialCategory();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("FinancialCategory updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("FinancialCategory created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save financialcategory:", error);
    toast.error(error.message || "Failed to save financialcategory");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit FinancialCategory": "Create FinancialCategory" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the financialcategory." : "Fill in the details to create a new financialcategory." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="type" > Type * </Label>

<Input id="type" {...register("type") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="color" > Color</Label>

<Input id="color" {...register("color") } />

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
