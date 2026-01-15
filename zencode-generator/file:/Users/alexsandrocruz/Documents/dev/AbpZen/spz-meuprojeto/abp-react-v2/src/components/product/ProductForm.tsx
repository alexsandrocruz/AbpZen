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
  import { useCreateProduct, useUpdateProduct } from "@/lib/abp/hooks/useProducts";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    sKU: z.any(),
  
    price: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ProductFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ProductForm({
  isOpen,
  onClose,
  initialValues,
}: ProductFormProps) {
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

  const createMutation = useCreateProduct();
const updateMutation = useUpdateProduct();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Product updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Product created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save product:", error);
    toast.error(error.message || "Failed to save product");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Product": "Create Product" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the product." : "Fill in the details to create a new product." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sKU" > SKU</Label>

<Input id="sKU" {...register("sKU") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="price" > Price * </Label>

<Input id="price" type = "number" step = "any" {...register("price") } />

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
