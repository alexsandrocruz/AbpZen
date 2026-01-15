import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetClientContactsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  clientId?: string;
  }

export function useClientContacts(input: GetClientContactsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["clientContacts", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/client-contact", {
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

export function useAllClientContacts() {
  return useQuery({
    queryKey: ["clientContacts", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/client-contact", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useClientContact(id: string) {
  return useQuery({
    queryKey: ["clientContact", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/client-contact/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateClientContact() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/client-contact", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["clientContacts"] });
    },
  });
}

export function useUpdateClientContact() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/client-contact/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["clientContacts"] });
      queryClient.invalidateQueries({ queryKey: ["clientContact", data.id] });
    },
  });
}

export function useDeleteClientContact() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/client-contact/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["clientContacts"] });
    },
  });
}


