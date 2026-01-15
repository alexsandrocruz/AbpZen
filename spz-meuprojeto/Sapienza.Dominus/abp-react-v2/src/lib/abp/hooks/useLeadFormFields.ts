import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadFormFieldsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadFormId?: string;
  }

export function useLeadFormFields(input: GetLeadFormFieldsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadFormFields", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-form-field", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllLeadFormFields() {
  return useQuery({
    queryKey: ["leadFormFields", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-form-field", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadFormField(id: string) {
  return useQuery({
    queryKey: ["leadFormField", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-form-field/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadFormField() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-form-field", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadFormFields"] });
    },
  });
}

export function useUpdateLeadFormField() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-form-field/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadFormFields"] });
      queryClient.invalidateQueries({ queryKey: ["leadFormField", data.id] });
    },
  });
}

export function useDeleteLeadFormField() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-form-field/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadFormFields"] });
    },
  });
}


